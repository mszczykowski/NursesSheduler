using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface INursesService
    {
        Task<IEnumerable<Nurse>> GetActiveDepartamentNurses(int departamentId);
        Task SetSpecialHoursBalance(Schedule schedule);
    }
}
