﻿namespace FastWiki.Service.Contracts.ChatApplication.Dto;

public class CreateChatDialogInput
{
    public string Name { get; set; }

    public string ChatApplicationId { get; set; }

    public string Description { get; set; } = string.Empty;
}