using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Veryfication
{
    internal static class AbsenceVeryficator
    {
        public static AbsenceVeryficationResult VerifyAbsence(AbsencesSummary absencesSummary, Absence absence)
        {
            if (absencesSummary.Year != absence.From.Year || absencesSummary.Year != absence.To.Year)
                return AbsenceVeryficationResult.InvalidYear;

            if (absencesSummary.Absences.Any(a => (
                        (a.From == absence.From || a.To == absence.From || a.From == absence.To || a.To == absence.To) 
                        || (a.From > absence.From && a.To < absence.From)
                        || (a.From > absence.To && a.To < absence.To)) && a.AbsenceId != absence.AbsenceId))
                return AbsenceVeryficationResult.AbsenceAlreadyExists;

            if (absencesSummary.Absences.Any(a => ((a.From == absence.From && a.To == absence.From)
                     || (a.From > absence.To && a.To < absence.To)) && a.AbsenceId != absence.AbsenceId))
                return AbsenceVeryficationResult.AbsenceAlreadyExists;

            return AbsenceVeryficationResult.Valid;
        }

    }
}
