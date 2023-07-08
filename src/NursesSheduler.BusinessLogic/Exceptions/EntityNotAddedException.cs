namespace NursesScheduler.BusinessLogic.Exceptions
{
    public sealed class EntityNotAddedException : Exception
    {
        public EntityNotAddedException() : base()
        {

        }
        public EntityNotAddedException(string type) : base($"Entity of type {type} not added.")
        {

        }
    }
}
