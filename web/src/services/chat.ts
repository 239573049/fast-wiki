import { PluginRequestPayload, createHeadersWithPluginSettings } from '@lobehub/chat-plugin-sdk';
import { produce } from 'immer';
import { merge } from 'lodash-es';

import { createErrorResponse } from '@/app/api/errorResponse';
import { INBOX_GUIDE_SYSTEMROLE } from '@/const/guide';
import { INBOX_SESSION_ID } from '@/const/session';
import { DEFAULT_AGENT_CONFIG } from '@/const/settings';
import { TracePayload, TraceTagMap } from '@/const/trace';
import { AgentRuntime, ChatCompletionErrorPayload, ModelProvider } from '@/libs/agent-runtime';
import { filesSelectors, useFileStore } from '@/store/file';
import { useSessionStore } from '@/store/session';
import { sessionMetaSelectors } from '@/store/session/selectors';
import { useUserStore } from '@/store/user';
import {
  modelConfigSelectors,
  modelProviderSelectors,
  preferenceSelectors,
  userProfileSelectors,
} from '@/store/user/selectors';
import { ChatErrorType } from '@/types/fetch';
import { ChatMessage, MessageToolCall } from '@/types/message';
import type { ChatStreamPayload, OpenAIChatMessage } from '@/types/openai/chat';
import { UserMessageContentPart } from '@/types/openai/chat';
import { FetchSSEOptions, fetchSSE, getMessageError } from '@/utils/fetch';
import { genToolCallingName } from '@/utils/toolCall';

import { createHeaderWithAuth, getProviderAuthPayload } from './_auth';
import { API_ENDPOINTS } from './_url';

interface FetchOptions extends FetchSSEOptions {
  isWelcomeQuestion?: boolean;
  signal?: AbortSignal | undefined;
  trace?: TracePayload;
}

interface GetChatCompletionPayload extends Partial<Omit<ChatStreamPayload, 'messages'>> {
  messages: ChatMessage[];
}

interface FetchAITaskResultParams extends FetchSSEOptions {
  abortController?: AbortController;
  onError?: (e: Error, rawError?: any) => void;
  /**
   * 加载状态变化处理函数
   * @param loading - 是否处于加载状态
   */
  onLoadingChange?: (loading: boolean) => void;
  /**
   * 请求对象
   */
  params: Partial<ChatStreamPayload>;
  trace?: TracePayload;
}

interface CreateAssistantMessageStream extends FetchSSEOptions {
  abortController?: AbortController;
  isWelcomeQuestion?: boolean;
  params: GetChatCompletionPayload;
  trace?: TracePayload;
}

/**
 * Initializes the AgentRuntime with the client store.
 * @param provider - The provider name.
 * @param payload - Init options
 * @returns The initialized AgentRuntime instance
 *
 * **Note**: if you try to fetch directly, use `fetchOnClient` instead.
 */
export function initializeWithClientStore(provider: string, payload: any) {
  // add auth payload
  const providerAuthPayload = getProviderAuthPayload(provider);
  const commonOptions = {
    // Some provider base openai sdk, so enable it run on browser
    dangerouslyAllowBrowser: true,
  };
  let providerOptions = {};

  providerOptions = {
    baseURL: providerAuthPayload?.endpoint,
  };
  /**
   * Configuration override order:
   * payload -> providerOptions -> providerAuthPayload -> commonOptions
   */
  return AgentRuntime.initializeWithProviderOptions(provider, {
    [provider]: {
      ...commonOptions,
      ...providerAuthPayload,
      ...providerOptions,
      ...payload,
    },
  });
}

class ChatService {
  createAssistantMessage = async (
    { plugins: enabledPlugins, messages, ...params }: GetChatCompletionPayload,
    options?: FetchOptions,
  ) => {
    const payload = merge(
      {
        model: DEFAULT_AGENT_CONFIG.model,
        stream: true,
        ...DEFAULT_AGENT_CONFIG.params,
      },
      params,
    );
    // ============  1. preprocess messages   ============ //

    const oaiMessages = this.processMessages(
      {
        messages,
        model: payload.model,
        tools: enabledPlugins,
      },
      options,
    );

    // check this model can use function call
    const canUseFC = modelProviderSelectors.isModelEnabledFunctionCall(payload.model)(
      useUserStore.getState(),
    );

    const shouldUseTools =  canUseFC;


    return this.getChatCompletion({ ...params, messages: oaiMessages }, options);
  };

  createAssistantMessageStream = async ({
    params,
    abortController,
    onAbort,
    onMessageHandle,
    onErrorHandle,
    onFinish,
    trace,
    isWelcomeQuestion,
  }: CreateAssistantMessageStream) => {
    await this.createAssistantMessage(params, {
      isWelcomeQuestion,
      onAbort,
      onErrorHandle,
      onFinish,
      onMessageHandle,
      signal: abortController?.signal,
      trace: this.mapTrace(trace, TraceTagMap.Chat),
    });
  };

  getChatCompletion = async (params: Partial<ChatStreamPayload>, options?: FetchOptions) => {
    const { signal } = options ?? {};

    const { provider = ModelProvider.OpenAI, ...res } = params;

    let model = res.model || DEFAULT_AGENT_CONFIG.model;

    const payload = merge(
      { model: DEFAULT_AGENT_CONFIG.model, stream: true, ...DEFAULT_AGENT_CONFIG.params },
      { ...res, model },
    );

    /**
     * Use browser agent runtime
     */
    const enableFetchOnClient = modelConfigSelectors.isProviderFetchOnClient(provider)(
      useUserStore.getState(),
    );

    let fetcher: typeof fetch | undefined = undefined;

    if (enableFetchOnClient) {
      /**
       * Notes:
       * 1. Browser agent runtime will skip auth check if a key and endpoint provided by
       *    user which will cause abuse of plugins services
       * 2. This feature will be disabled by default
       */
      fetcher = async () => {
        try {
          return await this.fetchOnClient({ payload, provider, signal });
        } catch (e) {
          const {
            errorType = ChatErrorType.BadRequest,
            error: errorContent,
            ...res
          } = e as ChatCompletionErrorPayload;

          const error = errorContent || e;
          // track the error at server side
          console.error(`Route: [${provider}] ${errorType}:`, error);

          return createErrorResponse(errorType, { error, ...res, provider });
        }
      };
    }

    const headers = await createHeaderWithAuth({
      headers: { 'Content-Type': 'application/json', },
      provider,
    });

    return fetchSSE(API_ENDPOINTS.chat(provider), {
      body: JSON.stringify(payload),
      fetcher: fetcher,
      headers,
      method: 'POST',
      onAbort: options?.onAbort,
      onErrorHandle: options?.onErrorHandle,
      onFinish: options?.onFinish,
      onMessageHandle: options?.onMessageHandle,
      signal,
    });
  };

  fetchPresetTaskResult = async ({
    params,
    onMessageHandle,
    onFinish,
    onError,
    onLoadingChange,
    abortController,
    trace,
  }: FetchAITaskResultParams) => {
    const errorHandle = (error: Error, errorContent?: any) => {
      onLoadingChange?.(false);
      if (abortController?.signal.aborted) {
        return;
      }
      onError?.(error, errorContent);
    };

    onLoadingChange?.(true);

    const data = await this.getChatCompletion(params, {
      onErrorHandle: (error) => {
        errorHandle(new Error(error.message), error);
      },
      onFinish,
      onMessageHandle,
      signal: abortController?.signal,
      trace: this.mapTrace(trace, TraceTagMap.SystemChain),
    }).catch(errorHandle);

    onLoadingChange?.(false);

    return await data?.text();
  };

  private processMessages = (
    {
      messages,
      tools,
      model,
    }: {
      messages: ChatMessage[];
      model: string;
      tools?: string[];
    },
    options?: FetchOptions,
  ): OpenAIChatMessage[] => {
    // handle content type for vision model
    // for the models with visual ability, add image url to content
    // refs: https://platform.openai.com/docs/guides/vision/quick-start
    const getContent = (m: ChatMessage) => {
      if (!m.files) return m.content;

      const imageList = filesSelectors.getImageUrlOrBase64ByList(m.files)(useFileStore.getState());

      if (imageList.length === 0) return m.content;

      const canUploadFile = modelProviderSelectors.isModelEnabledUpload(model)(
        useUserStore.getState(),
      );

      if (!canUploadFile) {
        return m.content;
      }

      return [
        { text: m.content, type: 'text' },
        ...imageList.map(
          (i) => ({ image_url: { detail: 'auto', url: i.url }, type: 'image_url' }) as const,
        ),
      ] as UserMessageContentPart[];
    };

    const postMessages = messages.map((m): OpenAIChatMessage => {
      switch (m.role) {
        case 'user': {
          return { content: getContent(m), role: m.role };
        }

        case 'assistant': {
          return {
            content: m.content,
            role: m.role,
            tool_calls: m.tools?.map(
              (tool): MessageToolCall => ({
                function: {
                  arguments: tool.arguments,
                  name: genToolCallingName(tool.identifier, tool.apiName, tool.type),
                },
                id: tool.id,
                type: 'function',
              }),
            ),
          };
        }

        case 'tool': {
          return {
            content: m.content,
            name: genToolCallingName(m.plugin!.identifier, m.plugin!.apiName, m.plugin?.type),
            role: m.role,
            tool_call_id: m.tool_call_id,
          };
        }

        default: {
          return { content: m.content, role: m.role };
        }
      }
    });

    return produce(postMessages, (draft) => {
      // if it's a welcome question, inject InboxGuide SystemRole
      const inboxGuideSystemRole =
        options?.isWelcomeQuestion &&
        options?.trace?.sessionId === INBOX_SESSION_ID &&
        INBOX_GUIDE_SYSTEMROLE;

        
      const injectSystemRoles = [inboxGuideSystemRole]
        .filter(Boolean)
        .join('\n\n');

      if (!injectSystemRoles) return;

      const systemMessage = draft.find((i) => i.role === 'system');

      if (systemMessage) {
        systemMessage.content = [systemMessage.content, injectSystemRoles]
          .filter(Boolean)
          .join('\n\n');
      } else {
        draft.unshift({
          content: injectSystemRoles,
          role: 'system',
        });
      }
    });
  };

  private mapTrace(trace?: TracePayload, tag?: TraceTagMap): TracePayload {
    const tags = sessionMetaSelectors.currentAgentMeta(useSessionStore.getState()).tags || [];

    const enabled = preferenceSelectors.userAllowTrace(useUserStore.getState());

    if (!enabled) return { ...trace, enabled: false };

    return {
      ...trace,
      enabled: true,
      tags: [tag, ...(trace?.tags || []), ...tags].filter(Boolean) as string[],
      userId: userProfileSelectors.userId(useUserStore.getState()),
    };
  }

  /**
   * Fetch chat completion on the client side.

   */
  private fetchOnClient = async (params: {
    payload: Partial<ChatStreamPayload>;
    provider: string;
    signal?: AbortSignal;
  }) => {
    const agentRuntime = await initializeWithClientStore(params.provider, params.payload);
    const data = params.payload as ChatStreamPayload;

    return agentRuntime.chat(data, { signal: params.signal });
  };
}

export const chatService = new ChatService();
