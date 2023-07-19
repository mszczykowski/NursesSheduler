using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Days.Queries.GetMonthDays;

namespace NursesScheduler.BlazorShared.Mappings
{
    internal sealed class DayViewModelMappings : Profile
    {
        public DayViewModelMappings()
        {
            CreateMap<GetMonthDaysResponse, DayViewModel>();
        }
    }
}
