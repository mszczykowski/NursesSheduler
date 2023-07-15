namespace NursesScheduler.BusinessLogic.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base()
        {

        }
        public EntityNotFoundException(string message) : base(message)
        {

        }
        public EntityNotFoundException(string key, string type) 
            : base($"Entity of type {type} with key {key} cannot be found.")
        {

        }
    }
}
