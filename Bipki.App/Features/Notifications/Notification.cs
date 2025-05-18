using System.Text.Json;

namespace Bipki.App.Features.Notifications;

public class Notification
{
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public string Url { get; set; } = "";

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public static class Templates
{
    public static Notification ActivityStart(string acivityName, string url = "") 
        => new ()
        {
            Title = $"{acivityName} скоро начнётся",
            Body = "Успевайте прийти!",
            Url = url
        };

    public static Notification ActivityNewDate(string activityName, DateTime newStart, string url = "") 
        => new() 
        {
            Title = $"Перенесли {activityName}",
            Body = $"Новое время начала {newStart:MM-dd HH:mm}",
            Url = url
        };
    
    public static Notification ConferenceNewDate(string conferenceName, DateTime newStart, string url = "") 
        => new() 
        {
            Title = $"{conferenceName} переносится",
            Body = $"Теперь {newStart:dd.MM.yyyy} в {newStart:HH:mm}",
            Url = url
        };
    
    public static Notification NewActivity(string activityName, string url = "") 
        => new() 
        {
            Title = $"Новая активность - {activityName}!",
            Body = $"Успевайте зарегистрироваться",
            Url = url
        };
    
    public static Notification ActivityDeleted(string activityName, string url = "") 
        => new() 
        {
            Title = $"{activityName} отменяется",
            Body = $"Но вы можете сходить куда-нибудь ещё",
            Url = url
        };
    
    public static Notification VerifyRegistration(string activityName, DateTime deadline, string url = "") 
        => new() 
        {
            Title = $"Освободилось место на {activityName}",
            Body = $"Успевайте подтвердить регистрацию до {deadline:MM.dd HH:mm}",
            Url = url
        };
    
    public static Notification ActivityShrunkSeats(string activityName, string url = "") 
        => new() 
        {
            Title = $"Мест на {activityName} стало меньше",
            Body = $"Перенесли вас в лист ожидания",
            Url = url
        };
}