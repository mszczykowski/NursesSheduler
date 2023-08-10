using AutoMapper;
using MediatR;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.ScheduleValidations.Queries.ValidateScheduleNurse
{
    internal class ValidateScheduleNurseQueryHandler : IRequestHandler<ValidateScheduleNurseRequest, 
        IEnumerable<ValidateScheduleNurseResponse>>
    {
        private readonly IMapper _mapper;

        public Task<IEnumerable<ValidateScheduleNurseResponse>> Handle(ValidateScheduleNurseRequest request, 
            CancellationToken cancellationToken)
        {
            var scheduleNurse = _mapper.Map<ScheduleNurse>(request.ScheduleNurse);

            throw new NotImplementedException();
        }
    }
}
