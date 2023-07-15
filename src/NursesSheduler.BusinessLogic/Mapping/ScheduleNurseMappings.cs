﻿using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.RecalculateScheduleHours;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleNurseMappings : Profile
    {
        public ScheduleNurseMappings()
        {
            CreateMap<ScheduleNurse, GetScheduleResponse.ScheduleNurseResponse>();

            CreateMap<RecalculateScheduleHoursRequest.ScheduleNurseRequest, ScheduleNurse>();
        }
    }
}
