using AutoMapper;
using NursesScheduler.BusinessLogic.Nurses.Commands.CreateNurse;
using NursesScheduler.BusinessLogic.Nurses.Queries.GetAllNurses;
using NursesScheduler.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class NurseMappings : Profile
    {
        public NurseMappings()
        {
            CreateMap<Nurse, CreateNurseRequest>()
                .ReverseMap();
            CreateMap<Nurse, CreateNurseResponse>()
                .ReverseMap();

            CreateMap<Nurse, GetAllNursesResponse>()
                .ReverseMap();
        }
    }
}
