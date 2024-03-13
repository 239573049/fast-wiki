namespace FastWiki.Service.Contracts.OpenAI;


public class ChatCompletionDto
{
    public string model { get; set; }

    public double? temperature { get; set; }

    /// <summary>
    /// ���¶Ȳ�������һ�ַ�����Ϊ�˲���������ģ�Ϳ��Ǿ���top_p����������token�Ľ�������0.1��ζ��ֻ���ǰ���ǰ10%���������ı�ǡ�����ͨ������ı�������¶ȡ��������������߶��ı䡣
    /// </summary>
    public double? top_p { get; set; }

    public bool stream { get; set; } = true;

    /// <summary>
    /// ���ɵĴ���������������Ĭ������£�ģ�Ϳ��Է��ص�token����Ϊ(4096 -��ʾtoken)��
    /// </summary>
    public double max_tokens { get; set; } = 2048;

    /// <summary>
    /// ��-2.0��2.0֮������֡���ֵ������±�����ı��д��ڵ�Ƶ�����ͷ����ǣ�����ģ�������ظ�ͬһ�еĿ����ԡ�[�й�Ƶ�ʺʹ��ڳͷ��ĸ�����Ϣ��](https://docs.api-reference/parameter -details)
    /// </summary>
    public double? frequency_penalty { get; set; }

    public OpenAIError OpenAiError { get; set; }

}

public class ChatInputCompletionDto : ChatCompletionDto
{
    /// <summary>
    /// ѡ��Ӧ�ó����ID�����δָ���������á�
    /// </summary>
    public long? applicationId { get; set; }

    /// <summary>
    /// ѡ��֪ʶ���ID�����δָ���������á�
    /// </summary>
    public long? userbankId { get; set; }
}

public class ChatCompletionDto<T> : ChatCompletionDto
{
    public List<T> messages { get; set; }
}

public class ChatCompletionRequestMessage
{
    public string role { get; set; }

    public string content { get; set; }

    public string? name { get; set; }
}

public class OpenAIError
{
    public string message { get; set; }
    public string type { get; set; }
    public object param { get; set; }
    public string code { get; set; }
}

public class ChatVisionCompletionRequestMessage
{
    public string role { get; set; }

    public ChatContent[] content { get; set; }

    public string? name { get; set; }
}

public class ChatContent
{
    public string type { get; set; }

    public string text { get; set; }

    public object image_url { get; set; }
}

public class ImageUrl
{
    public string detail { get; set; }
    
    public string url { get; set; }
}