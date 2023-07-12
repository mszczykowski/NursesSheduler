using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Commands.UpsertQuarter;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal class QuarterMappings : Profile
    {
        public QuarterMappings()
        {
            CreateMap<UpsertQuarterResponse, QuarterViewModel>();
        }
    }
}
