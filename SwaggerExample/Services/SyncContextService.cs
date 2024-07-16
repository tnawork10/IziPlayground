
namespace SwaggerExample.Services
{
    public class SyncContextService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var context = SynchronizationContext.Current;
            Console.WriteLine($"{context?.GetType().AssemblyQualifiedName ?? @"IS NULL at begin"}");
            return Task.Run(async () =>
            {
                Console.WriteLine($"{context?.GetType().AssemblyQualifiedName ?? @"IS NULL at Task before await"}");
                await Task.Delay(1000).ConfigureAwait(false);
                Console.WriteLine($"{context?.GetType().AssemblyQualifiedName ?? @"IS NULL at Task after await"}");
            });
        }
    }
}
