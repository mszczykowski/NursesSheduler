using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Managers;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Queries.GetDepartamentSettings
{
    internal sealed class GetDepartamentSettingsQueryHandler : IRequestHandler<GetDepartamentSettingsRequest, 
                                                                                        GetDepartamentSettingsResponse>
    {
        private readonly IDepartamentSettingsManager _departamentSettingsManager;
        private readonly IMapper _mapper;

        public GetDepartamentSettingsQueryHandler(IDepartamentSettingsManager departamentSettingsManager, IMapper mapper)
        {
            _departamentSettingsManager = departamentSettingsManager;
            _mapper = mapper;
        }

        public async Task<GetDepartamentSettingsResponse> Handle(GetDepartamentSettingsRequest request, 
                                                                                    CancellationToken cancellationToken)
        {
            return _mapper.Map<GetDepartamentSettingsResponse>(await _departamentSettingsManager
                .GetDepartamentSettings(request.DepartamentId));
        }
    }
}
