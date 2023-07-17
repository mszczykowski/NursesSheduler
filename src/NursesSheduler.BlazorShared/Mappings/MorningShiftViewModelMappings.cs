using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands.UpsertMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.CalculateMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.GetMorningShifts;

namespace NursesScheduler.BlazorShared.Mappings
{
    internal sealed class MorningShiftViewModelMappings : Profile
    {
        public MorningShiftViewModelMappings()
        {
            CreateMap<GetMorningShiftsResponse, MorningShiftViewModel>();

            CreateMap<CalculateMorningShiftsResponse, MorningShiftViewModel>();

            CreateMap<MorningShiftViewModel, UpsertMorningShiftsRequest.MorningShiftRequest>();
            CreateMap<UpsertMorningShiftsResponse, MorningShiftViewModel>();
        }
    }
}
