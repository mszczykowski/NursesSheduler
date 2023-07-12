﻿using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Days.Queries.GetMonthDays;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class DayViewModelMappings : Profile
    {
        public DayViewModelMappings()
        {
            CreateMap<GetMonthDaysResponse, DayViewModel>();
        }
    }
}
