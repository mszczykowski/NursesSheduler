namespace NursesScheduler.BlazorShared.Exceptions
{
    internal sealed class EntityNotAddedException : Exception
    {
        public EntityNotAddedException() : base()
        {

        }
        public EntityNotAddedException(string type) : base($"Entity of type {type} cannot be added.")
        {

        }
    }
}
