﻿using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.CloseSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.SolveSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class ScheduleViewModelMappings : Profile
    {
        public ScheduleViewModelMappings()
        {
            CreateMap<GetScheduleResponse, ScheduleViewModel>();

            CreateMap<BuildScheduleResponse, ScheduleViewModel>();

            CreateMap<ScheduleViewModel, GetScheduleStatsFromScheduleRequest.ScheduleRequest>();

            CreateMap<ScheduleViewModel, SolveScheduleRequest.ScheduleRequest>();

            CreateMap<ScheduleViewModel, UpsertScheduleRequest>();
            CreateMap<UpsertScheduleResponse, ScheduleViewModel>();

            CreateMap<ScheduleViewModel, CloseScheduleRequest>();
        }
    }
}
