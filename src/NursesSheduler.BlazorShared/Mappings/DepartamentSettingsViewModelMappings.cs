using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Commands.EditDepartamentSettings;
using NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Queries.GetDepartamentSettings;

namespace NursesScheduler.BlazorShared.Mappings
{
    internal sealed class DepartamentSettingsViewModelMappings : Profile
    {
        public DepartamentSettingsViewModelMappings()
        {
            CreateMap<GetDepartamentSettingsResponse, DepratamentSettingsViewModel>();

            CreateMap<DepratamentSettingsViewModel, EditDepartamentSettingsRequest>();
            CreateMap<EditDepartamentSettingsResponse, DepratamentSettingsViewModel>();
        }
    }
}
