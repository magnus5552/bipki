namespace Bipki.Database.Models;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Telegram { get; set; } = null!;

    public Guid ConferenceId { get; set; }
}