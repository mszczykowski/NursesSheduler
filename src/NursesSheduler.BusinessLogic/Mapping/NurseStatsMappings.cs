﻿using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.RecalculateNurseQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class NurseScheduleStatsMappings : Profile
    {
        public NurseScheduleStatsMappings()
        {
            CreateMap<NurseStats, GetScheduleStatsResponse.NurseStatsResponse>();
            CreateMap<NurseStats, RecalculateNursesScheduleStatsResponse>();

            CreateMap<GetQuarterStatsRequest.NurseStatsRequest, NurseScheduleStats>();
            CreateMap<NurseStats, GetQuarterStatsResponse.NurseStatsResponse>();

            CreateMap<RecalculateNurseQuarterStatsRequest.NurseStatsRequest, NurseStats>();
            CreateMap<NurseStats, RecalculateNurseQuarterStatsResponse>();
        }
    }
}
