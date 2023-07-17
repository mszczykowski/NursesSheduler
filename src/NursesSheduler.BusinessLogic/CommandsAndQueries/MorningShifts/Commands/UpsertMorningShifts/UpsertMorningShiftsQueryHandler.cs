using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands.UpsertMorningShifts
{
    internal sealed class UpsertMorningShiftsQueryHandler : IRequestHandler<UpsertMorningShiftsRequest,
        IEnumerable<UpsertMorningShiftsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<MorningShift> _validator;
        private readonly IApplicationDbContext _context;

        public UpsertMorningShiftsQueryHandler(IMapper mapper, IValidator<MorningShift> validator, 
            IApplicationDbContext context)
        {
            _mapper = mapper;
            _validator = validator;
            _context = context;
        }

        public async Task<IEnumerable<UpsertMorningShiftsResponse>> Handle(UpsertMorningShiftsRequest request,
            CancellationToken cancellationToken)
        {
            var morningShifts = _mapper.Map<IEnumerable<MorningShift>>(request.MorningShifts);

            foreach(var morningShift in morningShifts)
            {
                var validationResult = await _validator.ValidateAsync(morningShift);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            var quarter = await _context.Quarters
                .Include(q => q.MorningShifts)
                .FirstOrDefaultAsync(q => q.QuarterId == request.QuarterId);

            if(quarter is null)
            {
                throw new EntityNotFoundException(request.QuarterId, nameof(Quarter));
            }

            for (int i = 0; i < Enum.GetValues<MorningShiftIndex>().Length; i++)
            {
                var morningShift = quarter.MorningShifts.FirstOrDefault(m => m.Index == (MorningShiftIndex)i);
                var updatedMorningShift = morningShifts.First(m => m.Index == (MorningShiftIndex)i);

                if (morningShift is not null)
                {
                    if(morningShift.ReadOnly)
                    {
                        throw new OperationNotPermittedException("Editing readonly morningShift");
                    }

                    morningShift.ShiftLength = updatedMorningShift.ShiftLength;
                }
                else
                {
                    quarter.MorningShifts.Add(new MorningShift
                    {
                        Index = (MorningShiftIndex)i,
                        ShiftLength = updatedMorningShift.ShiftLength,
                    });
                }
            }

            _quarterProvider.InvalidateCache(request.QuarterId);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<IEnumerable<UpsertMorningShiftsResponse>>(quarter.MorningShifts) : null;
        }
    }
}
