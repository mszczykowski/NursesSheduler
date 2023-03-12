using NursesScheduler.Domain.DomainModels;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Veryfication
{
    internal static class AbsenceVeryficator
    {
        public static AbsenceVeryficationResult VerifyAbsence(YearlyAbsencesSummary yearlyAbsences, Absence absence)
        {
            if (yearlyAbsences.Year != absence.From.Year || yearlyAbsences.Year != absence.To.Year)
                return AbsenceVeryficationResult.InvalidYear;

            if (yearlyAbsences.Absences.Any(a => (
                        (a.From == absence.From || a.To == absence.From || a.From == absence.To || a.To == absence.To) 
                        || (a.From > absence.From && a.To < absence.From)
                        || (a.From > absence.To && a.To < absence.To)) && a.AbsenceId != absence.AbsenceId))
                return AbsenceVeryficationResult.AbsenceAlreadyExists;

            if (yearlyAbsences.Absences.Any(a => ((a.From == absence.From && a.To == absence.From)
                     || (a.From > absence.To && a.To < absence.To)) && a.AbsenceId != absence.AbsenceId))
                return AbsenceVeryficationResult.AbsenceAlreadyExists;

            return AbsenceVeryficationResult.Valid;
        }

    }
}
