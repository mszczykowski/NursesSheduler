namespace NursesScheduler.BusinessLogic.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException() : base()
        {

        }
        public EntityAlreadyExistsException(string message) : base(message)
        {

        }
        public EntityAlreadyExistsException(int id, string type) : base($"Entity of type {type} with key {id} already exists.")
        {

        }
    }
}
