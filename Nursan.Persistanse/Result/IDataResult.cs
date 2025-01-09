namespace Nursan.Persistanse.Result
{
    public interface IDataResult<T> : IResults
    {
        T Data { get; }
    }
}
