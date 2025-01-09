namespace Nursan.Persistanse.Result
{
    public class SuccessResults : Result
    {
        public SuccessResults(string message) : base(true, message)
        {

        }
        public SuccessResults() : base(true)
        {

        }
    }
}
