using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class NurseStatsViewModelMappings : Profile
    {
        public NurseStatsViewModelMappings()
        {
            CreateMap<GetScheduleStatsResponse.NurseStatsResponse, NurseStatsViewModel>();
            CreateMap<GetScheduleStatsResponse.NurseStatsResponse, KeyValuePair<int, NurseStatsViewModel>>()
                .ConstructUsing((x, mapper) => new KeyValuePair<int, NurseStatsViewModel>(x.NurseId, mapper.Mapper
                    .Map<NurseStatsViewModel>(x)));

            CreateMap<GetScheduleStatsFromScheduleResponse.NurseStatsResponse, NurseStatsViewModel>();
            CreateMap<GetScheduleStatsFromScheduleResponse.NurseStatsResponse, KeyValuePair<int, NurseStatsViewModel>>()
                .ConstructUsing((x, mapper) => new KeyValuePair<int, NurseStatsViewModel>(x.NurseId, mapper.Mapper
                    .Map<NurseStatsViewModel>(x)));

            CreateMap<NurseStatsViewModel, GetScheduleStatsFromScheduleResponse.NurseStatsResponse>();

            CreateMap<GetQuarterStatsResponse.NurseStatsResponse, NurseStatsViewModel>();
            CreateMap<GetQuarterStatsResponse.NurseStatsResponse, KeyValuePair<int, NurseStatsViewModel>>()
                .ConstructUsing((x, mapper) => new KeyValuePair<int, NurseStatsViewModel>(x.NurseId, mapper.Mapper
                    .Map<NurseStatsViewModel>(x)));

            CreateMap<NurseStatsViewModel, GetQuarterStatsRequest.NurseStatsRequest>();
            CreateMap<KeyValuePair<int, NurseStatsViewModel>, GetQuarterStatsRequest.NurseStatsRequest>()
                .ConstructUsing((x, mapper) => mapper.Mapper.Map<GetQuarterStatsRequest.NurseStatsRequest>(x.Value));

            CreateMap<RecalculateNurseStatsResponse.NursesStatsResponse, NurseStatsViewModel>();
        }
    }
}
