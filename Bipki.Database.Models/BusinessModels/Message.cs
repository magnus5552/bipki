﻿using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class Message : Entity
{
    public DateTime Timestamp { get; set; }

    public string Text { get; set; } = null!;
    
    public Guid ChatId { get; set; }
    
    public Guid SenderId { get; set; }

    public string SenderName { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;
}