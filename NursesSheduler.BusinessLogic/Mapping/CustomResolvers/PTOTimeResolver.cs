using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.EditAbsencesSummary;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Mapping.CustomResolvers
{
    internal sealed class PTOTimeResolver : IValueResolver<EditAbsencesSummaryRequest, AbsencesSummary, TimeSpan>
    {
        public TimeSpan Resolve(EditAbsencesSummaryRequest source, AbsencesSummary destination, TimeSpan destMember, 
                                ResolutionContext context)
        {
            return source.PTODays * TimeSpan.FromDays(1);
        }
    }
}
