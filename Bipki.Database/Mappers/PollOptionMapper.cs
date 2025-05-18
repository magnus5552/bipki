using Bipki.Database.Models;
using DbPollOption = Bipki.Database.Models.BusinessModels.PollOption;

namespace Bipki.Database.Mappers;

public static class PollOptionMapper
{
    public static PollOption? Map(DbPollOption? option) => option is null
        ? null
        : new PollOption
        {
            PollId = option.PollId,
            Text = option.Text,
            Id = option.Id
        };
    
    public static DbPollOption? Map(PollOption? option) => option is null
        ? null
        : new DbPollOption
        {
            PollId = option.PollId,
            Text = option.Text,
            Id = option.Id
        };
}