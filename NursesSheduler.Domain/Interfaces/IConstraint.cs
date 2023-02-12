using NursesScheduler.Domain.DatabaseModels;

namespace NursesScheduler.Domain.Interfaces
{
    public interface IConstraint
    {
        public bool IsEnabled { get; set; }
        public int DepartamentId { get; set; }
        public Departament Departament { get; set; }
    }
}
