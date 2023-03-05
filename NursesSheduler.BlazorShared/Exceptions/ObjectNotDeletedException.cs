namespace NursesScheduler.BlazorShared.Exceptions
{
    internal class ObjectNotDeletedException : Exception
    {
        public ObjectNotDeletedException() : base()
        {

        }
        public ObjectNotDeletedException(string message) : base(message)
        {

        }
        public ObjectNotDeletedException(int id, string type) : base($"Entity of type {type} with key {id} cannot be deleted.")
        {

        }
    }
}
