
namespace Service.Slave.BackgroundServices
{
    public class MasterPollingService(ServiceMasterApi api) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"Begin MasterPollingService");

            if (false)
            {
                var stream1 = api.RequestPartialWithGet<long>();

                await foreach (var item in stream1)
                {
                    Console.WriteLine($"With GET: {item}");
                }
            }
            var stream = api.RequestPartialWithPost<long>();

            Console.WriteLine($"After stream recieved");

            await foreach (var item in stream)
            {
                Console.WriteLine($"With Post: {item}");
            }
        }
    }
}
