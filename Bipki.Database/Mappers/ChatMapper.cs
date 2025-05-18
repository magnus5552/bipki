using Bipki.Database.Models;
using DbChatType = Bipki.Database.Models.BusinessModels.ChatType;
using DbChat = Bipki.Database.Models.BusinessModels.Chat;

namespace Bipki.Database.Mappers;

public static class ChatMapper
{
    public static Chat? Map(DbChat? chat)
        => chat is null
            ? null
            : new Chat
            {
                Title = chat.Title,
                Type = Map(chat.Type),
                Id = chat.Id
            };

    public static DbChat? Map(Chat? chat)
        => chat is null
            ? null
            : new DbChat
            {
                Title = chat.Title,
                Type = Map(chat.Type),
                Id = chat.Id
            };

    private static ChatType Map(DbChatType type) =>
        type switch
        {
            DbChatType.Activity => ChatType.Activity,
            DbChatType.Conference => ChatType.Conference,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    private static DbChatType Map(ChatType type) =>
        type switch
        {
            ChatType.Activity => DbChatType.Activity,
            ChatType.Conference => DbChatType.Conference,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
}