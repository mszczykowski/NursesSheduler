namespace NursesScheduler.Domain.Exceptions
{
    public sealed class OperationNotPermittedException : Exception
    {
        public OperationNotPermittedException(string operation) : base($"Operation: {operation} is not permitted.")
        {

        }
    }
}
