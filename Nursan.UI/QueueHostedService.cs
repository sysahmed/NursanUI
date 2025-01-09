using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nursan.Domain.SystemClass;
using Nursan.Persistanse.Extensions;

namespace Nursan.UI
{
    public class QueueHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue<BaseEntity> _servicesQueue;
        private readonly ILogger<QueueHostedService> _logger;
        public QueueHostedService(ILogger<QueueHostedService> logger, IBackgroundTaskQueue<BaseEntity> servicesQueue)
        {
            _logger = logger;
            _servicesQueue = servicesQueue;

        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {

                var name = await _servicesQueue.DeQueue(cancellationToken);
                _logger.LogInformation($"ExecuteAsync worker for {name}");
                Console.WriteLine(name);
                Console.ReadLine();

            }
        }
    }
}
