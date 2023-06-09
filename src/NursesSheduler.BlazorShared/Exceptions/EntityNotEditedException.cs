namespace NursesScheduler.BlazorShared.Exceptions
{
    internal sealed class EntityNotEditedException : Exception
    {
        public EntityNotEditedException() : base()
        {

        }
        public EntityNotEditedException(string message) : base(message)
        {

        }
        public EntityNotEditedException(int id, string type) : base($"Entity of type {type} with key {id} cannot be edited.")
        {

        }
    }
}
