using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.RecalculateNurseQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;

namespace NursesScheduler.BlazorShared.Mappings
{
    internal sealed class NurseStatsViewModelMappings : Profile
    {
        public NurseStatsViewModelMappings()
        {
            CreateMap<GetScheduleStatsResponse.NurseStatsResponse, NurseStatsViewModel>();
            CreateMap<GetScheduleStatsResponse.NurseStatsResponse, KeyValuePair<int, NurseStatsViewModel>>()
                .ConstructUsing((x, mapper) => new KeyValuePair<int, NurseStatsViewModel>(x.NurseId, mapper.Mapper
                    .Map<NurseStatsViewModel>(x)));

            CreateMap<NurseStatsViewModel, GetScheduleStatsResponse.NurseStatsResponse>();

            CreateMap<GetQuarterStatsResponse.NurseStatsResponse, NurseStatsViewModel>();
            CreateMap<GetQuarterStatsResponse.NurseStatsResponse, KeyValuePair<int, NurseStatsViewModel>>()
                .ConstructUsing((x, mapper) => new KeyValuePair<int, NurseStatsViewModel>(x.NurseId, mapper.Mapper
                    .Map<NurseStatsViewModel>(x)));

            CreateMap<NurseStatsViewModel, RecalculateNurseQuarterStatsRequest.NurseStatsRequest>();
            
            CreateMap<NurseStatsViewModel, GetQuarterStatsRequest.NurseStatsRequest>();
            CreateMap<KeyValuePair<int, NurseStatsViewModel>, GetQuarterStatsRequest.NurseStatsRequest>()
                .ConstructUsing((x, mapper) => mapper.Mapper.Map<GetQuarterStatsRequest.NurseStatsRequest>(x.Value));

            CreateMap<RecalculateNursesScheduleStatsResponse, NurseStatsViewModel>();
            CreateMap<RecalculateNurseQuarterStatsResponse, NurseStatsViewModel>();
        }
    }
}
