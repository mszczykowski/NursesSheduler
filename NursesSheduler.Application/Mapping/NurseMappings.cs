using AutoMapper;
using NursesSheduler.Application.Nurses.Commands.CreateNurse;
using NursesSheduler.Application.Nurses.Queries.GetAllNurses;
using NursesSheduler.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesSheduler.Application.Mapping
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
