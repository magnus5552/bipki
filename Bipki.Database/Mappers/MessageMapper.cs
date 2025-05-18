using Bipki.Database.Models;
using DbMessage = Bipki.Database.Models.BusinessModels.Message;

namespace Bipki.Database.Mappers;

public static class MessageMapper
{
    public static Message? Map(DbMessage? message)
        => message is null
            ? null
            : new Message
            {
                Timestamp = message.Timestamp,
                Text = message.Text,
                ChatId = message.ChatId,
                SenderId = message.SenderId,
                SenderName = message.SenderName,
                Id = message.Id
            };

    public static DbMessage? Map(Message? message)
        => message is null
            ? null
            : new DbMessage
            {
                Id = message.Id,
                Timestamp = message.Timestamp,
                Text = message.Text,
                ChatId = message.ChatId,
                SenderId = message.SenderId,
                SenderName = message.SenderName
            };
}