using Bipki.Database.Mappers;
using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public class MessageRepository: IMessageRepository
{
    private readonly BipkiContext dbContext;

    public MessageRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task Add(Message message)
    {
        var dbMessage = MessageMapper.Map(message);
        if (dbMessage is null)
        {
            return;
        }
        
        await dbContext.AddAsync(dbMessage).ConfigureAwait(false);
        await dbContext.SaveChangesAsync();
    }

    public IEnumerable<Message> GetByChatId(Guid chatId)
    {
        return dbContext.Messages
            .Where(x => x.ChatId == chatId)
            .AsEnumerable()
            .Select(MessageMapper.Map)
            .Where(x => x is not null)!;
    }
}