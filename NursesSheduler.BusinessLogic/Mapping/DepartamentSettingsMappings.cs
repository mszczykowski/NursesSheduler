using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Commands.EditDepartamentSettings;
using NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Queries.GetDepartamentSettings;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class DepartamentSettingsMappings : Profile
    {
        public DepartamentSettingsMappings()
        {
            CreateMap<DepartamentSettings, GetDepartamentSettingsResponse>();

            CreateMap<EditDepartamentSettingsRequest, DepartamentSettings>();
            CreateMap<DepartamentSettings, EditDepartamentSettingsResponse>();
        }
    }
}
