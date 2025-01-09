namespace Nursan.Persistanse.Extensions
{
    public interface IBackgroundTaskQueue<T>
    {
        ValueTask AddQueue(T item);
        ValueTask UpdateQueue(T item);
        ValueTask DeleteQueue(T item);
        ValueTask<T> DeQueue(CancellationToken cancellationToken);
    }
}
