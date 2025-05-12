using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.InsuranceTypes
{
    public class AddInsuranceTypeCommand : ICommand<IResponse<InsuranceType>>
    {
        public required string Name { get; set; }
        public required int Id { get; set; }
        public List<Insurance>? Insurances { get; set; }
    }

    [UsedImplicitly]
    internal class AddInsuranceTypeCommandValidator : AbstractValidator<InsuranceType>
    {
        public AddInsuranceTypeCommandValidator()
        {
            RuleFor(command => command.Name).MaximumLength(50);
        }
    }

    internal class AddInsuranceTypeCommandHandler : ICommandHandler<AddInsuranceTypeCommand, IResponse<Domain.Entities.InsuranceType>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddInsuranceTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<Domain.Entities.InsuranceType>> Handle(AddInsuranceTypeCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.InsuranceType>(command);

            _unitOfWork.InsuranceTypes.Add(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Created<Domain.Entities.InsuranceType>($"for feed {command.Id}")).For(entity);
        }
    }
}
