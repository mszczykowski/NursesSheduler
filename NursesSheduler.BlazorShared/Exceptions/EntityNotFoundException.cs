namespace NursesScheduler.BlazorShared.Exceptions
{
    internal sealed class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base()
        {

        }
        public EntityNotFoundException(string message) : base(message)
        {

        }
        public EntityNotFoundException(int id, string type) : base($"Entity of type {type} with key {id} cannot be found.")
        {

        }
    }
}
