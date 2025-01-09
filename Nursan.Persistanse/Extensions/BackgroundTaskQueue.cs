using Microsoft.Extensions.Configuration;
using Nursan.Domain.Entity;
using Nursan.Domain.SystemClass;
using System.Threading.Channels;

namespace Nursan.Persistanse.Extensions
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue<BaseEntity>
    {

        Nursan.Persistanse.UnitOfWork.UnitOfWork unitOfWork;
        private readonly Channel<BaseEntity> _queue;
        private readonly IConfiguration _configuration;
        public BackgroundTaskQueue(IConfiguration configuration)
        {
            _configuration = configuration;
            unitOfWork = new Nursan.Persistanse.UnitOfWork.UnitOfWork(new UretimOtomasyonContext());
            int.TryParse(configuration["QueueCapacity"], out int capasity);
            BoundedChannelOptions options = new(capasity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<BaseEntity>(options);
        }
        public async ValueTask DeleteQueue(BaseEntity item)
        {
            ArgumentNullException.ThrowIfNull(item);
            unitOfWork.GetRepository<BaseEntity>().Add(item);
            await _queue.Writer.WriteAsync(item);
        }
        public async ValueTask UpdateQueue(BaseEntity item)
        {
            ArgumentNullException.ThrowIfNull(item);
            unitOfWork.GetRepository<BaseEntity>().Add(item);
            await _queue.Writer.WriteAsync(item);
        }
        public async ValueTask AddQueue(BaseEntity item)
        {
            ArgumentNullException.ThrowIfNull(item);
            unitOfWork.GetRepository<BaseEntity>().Add(item);
            await _queue.Writer.WriteAsync(item);
        }
        public ValueTask<BaseEntity> DeQueue(CancellationToken cancellationToken)
        {
            var item = _queue.Reader.ReadAsync(cancellationToken);
            return item;
        }
    }
}
