using Bipki.Database.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;

namespace Bipki.Database.Repositories;

public class ChatRepository: IChatRepository
{
    private readonly BipkiContext dbContext;

    public ChatRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<Chat?> GetById(Guid id)
    {
        return await dbContext.Chats.FirstOrDefaultAsync(x => x.Id == id);
    }
}