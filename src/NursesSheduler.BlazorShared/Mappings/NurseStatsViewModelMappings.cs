using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.RecalculateNurseQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;

namespace NursesScheduler.BlazorShared.Mappings
{
    internal sealed class NurseStatsViewModelMappings : Profile
    {
        public NurseStatsViewModelMappings()
        {
            CreateMap<GetScheduleStatsResponse.NurseStatsResponse, NurseStatsViewModel>();

            CreateMap<NurseStatsViewModel, GetScheduleStatsResponse.NurseStatsResponse>();

            CreateMap<GetQuarterStatsResponse.NurseStatsResponse, NurseStatsViewModel>();

            CreateMap<NurseStatsViewModel, RecalculateNurseQuarterStatsRequest.NurseStatsRequest>();
        }
    }
}
