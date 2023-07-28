﻿using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleMappings : Profile
    {
        public ScheduleMappings()
        {
            CreateMap<Schedule, BuildScheduleResponse>();

            CreateMap<Schedule, GetScheduleResponse>();

            CreateMap<GetScheduleStatsFromScheduleRequest.ScheduleRequest, Schedule>();

            CreateMap<UpsertScheduleRequest, Schedule>();
            CreateMap<Schedule, UpsertScheduleResponse>();
        }
    }
}
