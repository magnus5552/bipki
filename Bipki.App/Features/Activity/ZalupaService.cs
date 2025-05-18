using System.Threading.Channels;
using Bipki.Database.Repositories;

namespace Bipki.App.Features.Activity;

public class ZalupaService
{
    private readonly IActivityRegistrationRepository activityRegistrationRepository;
    private readonly Channel<DeleteEvent> channel;

    public ZalupaService(IActivityRegistrationRepository activityRegistrationRepository)
    {
        this.activityRegistrationRepository = activityRegistrationRepository;
        channel = Channel.CreateBounded<DeleteEvent>(new BoundedChannelOptions(15));
    }

    public async Task Start(CancellationToken cancellationToken)
    {
        var tasks = new[]
        {
            RunFetchingDeletes(cancellationToken),
            RunProcessingDeletes(cancellationToken)
        };

        try
        {
            await Task.WhenAny(tasks);
        }
        finally
        {
            await Task.WhenAll(tasks);
        }
    }

    private async Task RunFetchingDeletes(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // Здесь какое нибудь foreach(var a in repository.GetNotAccepted()) репо возвращает енумербл
            // внутри цикла какая нить проверка которая тебе нужна по типу DateTime.UTCNow - x.Date < DateTime.FromMinutes(15)
            // и потом если прошло то await channel.Writer.WriteAsync(new DeleteEvent());
            await channel.Writer.WriteAsync(new DeleteEvent(), cancellationToken);
            // после того как он пробежит весь цикл, ОБЯЗТЕЛЬНО че нить такое:
            await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
            Console.WriteLine("Я работаю!!!");
        }
    }

    private async Task RunProcessingDeletes(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var deleteEvent = await channel.Reader.ReadAsync(cancellationToken);
            // И обрабатываешь как душе угодно
        }
    }

    class DeleteEvent
    {
        // Поля нужные, чтобы перекидывать между джобами
    }
}