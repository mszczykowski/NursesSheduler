namespace NursesScheduler.Domain.Exceptions
{
    public sealed class EntityNotDeletedException : Exception
    {
        public EntityNotDeletedException() : base()
        {

        }
        public EntityNotDeletedException(string message) : base(message)
        {

        }
        public EntityNotDeletedException(int id, string type) :
            base($"Entity of type {type} with key {id} cannot be deleted.")
        {

        }
    }
}
