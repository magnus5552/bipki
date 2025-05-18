using System.Threading.Channels;
using Bipki.Database.Repositories;
using TL;
using Channel = System.Threading.Channels.Channel;

namespace Bipki.App.Features.Activity;

public class ZalupaService
{
    private readonly IActivityRegistrationRepository activityRegistrationRepository;
    private readonly RegistrationsManager registrationsManager;
    private readonly Channel<DeleteEvent> channel;

    public ZalupaService(IActivityRegistrationRepository activityRegistrationRepository, RegistrationsManager registrationsManager)
    {
        this.activityRegistrationRepository = activityRegistrationRepository;
        this.registrationsManager = registrationsManager;
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

            foreach (var unverifiedRegistration in await activityRegistrationRepository.GetAllUnverified())
            {
                if (unverifiedRegistration.RegisteredAt + TimeSpan.FromMinutes(15) < DateTime.Now)
                    await channel.Writer.WriteAsync(new DeleteEvent { Id = unverifiedRegistration.Id }, cancellationToken);
            }
            
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
            await registrationsManager.DeleteUnverifiedRegistration(deleteEvent.Id);
        }
    }

    class DeleteEvent
    {
        public Guid Id { get; set; }
    }
}