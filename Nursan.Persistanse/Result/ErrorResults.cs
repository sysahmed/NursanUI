namespace Nursan.Persistanse.Result
{
    public class ErrorResult : Result
    {
        public ErrorResult(bool v, string message) : base(false, message)
        {
        }

        public ErrorResult(string message) : base(false, message)
        {
        }

        public ErrorResult() : base(false)
        {
        }
    }
}
