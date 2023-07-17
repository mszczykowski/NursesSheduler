using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Commands.AddQuarter;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Queries.GetQuarter;

namespace NursesScheduler.BlazorShared.Mappings
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
