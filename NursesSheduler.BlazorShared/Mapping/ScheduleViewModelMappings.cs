﻿using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class ScheduleViewModelMappings : Profile
    {
        public ScheduleViewModelMappings()
        {
            CreateMap<GetScheduleResponse, ScheduleViewModel>();
        }
    }
}
