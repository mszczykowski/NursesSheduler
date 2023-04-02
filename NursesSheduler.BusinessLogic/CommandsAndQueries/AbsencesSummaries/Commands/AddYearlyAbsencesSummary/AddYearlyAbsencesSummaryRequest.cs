using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.AddYearlyAbsencesSummary
{
    public sealed class AddYearlyAbsencesSummaryRequest : IRequest<AddYearlyAbsencesSummaryResponse>
    {
        public int Year { get; set; }
        public int NurseId { get; set; }
    }
}
