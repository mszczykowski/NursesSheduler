using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.EditNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNursesFromDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class NurseMappings : Profile
    {
        public NurseMappings()
        {
            CreateMap<AddNurseRequest, Nurse>();
            CreateMap<Nurse, AddNurseResponse>();
            CreateMap<Nurse, GetNursesFromDepartamentResponse>();
            CreateMap<Nurse, GetNurseResponse>();
            CreateMap<EditNurseRequest, Nurse>();
            CreateMap<Nurse, EditNurseResponse>();
            CreateMap<Nurse, GetAbsencesSummaryByDepartamentResponse>();
            CreateMap<Nurse, GetScheduleResponse.NurseResponse>();
        }
    }
}
