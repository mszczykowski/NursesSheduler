using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;

namespace NursesScheduler.BlazorShared.Mappings
{
    internal sealed class NurseWorkDayViewModelMappings : Profile
    {
        public NurseWorkDayViewModelMappings()
        {
            CreateMap<BuildScheduleResponse.NurseWorkDayResponse, NurseWorkDayViewModel>();

            CreateMap<NurseWorkDayViewModel, GetScheduleStatsFromScheduleRequest.NurseWorkDayRequest>();

            CreateMap<NurseWorkDayViewModel, RecalculateNurseScheduleStatsRequest.NurseWorkDayRequest>();
        }
    }
}
