using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Commands.UpsertQuarter;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class QuarterMappings : Profile
    {
        public QuarterMappings()
        {
            CreateMap<Quarter, UpsertQuarterResponse>();
        }
    }
}
