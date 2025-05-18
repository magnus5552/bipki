using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IMessageRepository
{
    Task Add(Message message);

    IEnumerable<Message> GetByChatId(Guid chatId);
}