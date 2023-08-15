using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Commands.AddQuarter;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Queries.GetQuarter;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class QuarterMappings : Profile
    {
        public QuarterMappings()
        {
            CreateMap<Quarter, AddQuarterResponse>();
            CreateMap<Quarter, GetQuarterResponse>();
        }
    }
}
