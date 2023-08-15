using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IActiveNursesService
    {
        Task<IEnumerable<Nurse>> GetActiveDepartamentNurses(int departamentId);
    }
}
