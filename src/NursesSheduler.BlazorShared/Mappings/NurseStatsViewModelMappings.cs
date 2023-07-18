﻿using AutoMapper;
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

            CreateMap<NurseStatsViewModel, GetScheduleStatsResponse.NurseStatsResponse>();

            CreateMap<GetQuarterStatsResponse.NurseStatsResponse, NurseStatsViewModel>();

            CreateMap<NurseStatsViewModel, RecalculateNurseQuarterStatsRequest.NurseStatsRequest>();
            CreateMap<NurseStatsViewModel, GetQuarterStatsRequest.NurseStatsRequest>();

            CreateMap<RecalculateNursesScheduleStatsResponse, NurseStatsViewModel>();
            CreateMap<RecalculateNurseQuarterStatsResponse, NurseStatsViewModel>();
        }
    }
}
