using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Commands.AddQuarter;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Queries.GetQuarter;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal class QuarterViewModelMappings : Profile
    {
        public QuarterViewModelMappings()
        {
            CreateMap<AddQuarterResponse, QuarterViewModel>();
            CreateMap<GetQuarterResponse, QuarterViewModel>();
        }
    }
}
