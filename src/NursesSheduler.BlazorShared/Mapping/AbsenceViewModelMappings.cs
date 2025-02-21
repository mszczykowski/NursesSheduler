﻿using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BlazorShared.Models.ViewModels.Forms;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Queries.GetAbsences;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummary;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class AbsenceViewModelMappings : Profile
    {
        public AbsenceViewModelMappings()
        {
            CreateMap<AbsenceFormViewModel, AddAbsenceRequest>();
            CreateMap<AbsenceFormViewModel, EditAbsenceRequest>();

            CreateMap<AddAbsenceResponse.AbsenceResponse, AbsenceViewModel>();

            CreateMap<EditAbsenceResponse.AbsenceResponse, AbsenceViewModel>();

            CreateMap<GetAbsencesSummaryResponse.AbsenceResponse, AbsenceViewModel>();

            CreateMap<GetAbsencesResponse, AbsenceViewModel>();
        }
    }
}
