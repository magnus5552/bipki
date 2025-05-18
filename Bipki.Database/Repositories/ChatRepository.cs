using Bipki.Database.Mappers;
using Bipki.Database.Models;
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
        var chat = await dbContext.Chats.FirstOrDefaultAsync(x => x.Id == id);
        return ChatMapper.Map(chat);
    }

    public async Task Add(Chat chat)
    {
        var dbChat = ChatMapper.Map(chat);
        if (dbChat is null)
        {
            return;
        }

        await dbContext.Chats.AddAsync(dbChat);
        await dbContext.SaveChangesAsync();
    }
}