using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.RecalculateAbsencesSummary
{
    public sealed class RecalculateAbsencesSummaryRequest : IRequest<RecalculateAbsencesSummaryResponse>
    {
        public int AbsencesSummaryId{ get; set; }
    }
}
