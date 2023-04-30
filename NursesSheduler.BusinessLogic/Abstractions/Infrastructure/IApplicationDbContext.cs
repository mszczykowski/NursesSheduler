using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<Absence> Absences { get; set; }
        DbSet<AbsencesSummary> AbsencesSummaries { get; set; }
        DbSet<Departament> Departaments { get; set; }
        DbSet<DepartamentSettings> DepartamentSettings { get; set; }
        DbSet<MorningShift> MorningShifts { get; set; }
        DbSet<Nurse> Nurses { get; set; }
        DbSet<NurseQuarterStats> NursesQuartersStats { get; set; }
        DbSet<NurseWorkDay> NursesWorkDays { get; set; }
        DbSet<Quarter> Quarters { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<ScheduleNurse> ScheduleNurses { get; set; }
        DbSet<WorkTimeInWeek> WorkTimeInWeeks { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry Entry(object entity);
    }
}
