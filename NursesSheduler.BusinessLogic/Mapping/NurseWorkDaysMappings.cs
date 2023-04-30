using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class NurseWorkDaysMappings : Profile
    {
        public NurseWorkDaysMappings()
        {
            CreateMap<NurseWorkDay, GetScheduleResponse.NurseWorkDayResponse>();
        }
    }
}
