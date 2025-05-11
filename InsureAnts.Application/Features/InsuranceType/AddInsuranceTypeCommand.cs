using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.InsuranceType
{
    public class AddInsuranceTypeCommand : ICommand<IResponse<InsureAnts.Domain.Entities.InsuranceType>>
    {
        public required string Name { get; set; }
        public required int Id { get; set; }
        public List<InsureAnts.Domain.Entities.Insurance>? Insurances { get; set; }
    }

    [UsedImplicitly]
    internal class AddInsuranceTypeCommandValidator : AbstractValidator<InsureAnts.Domain.Entities.InsuranceType>
    {
        public AddInsuranceTypeCommandValidator()
        {
            RuleFor(command => command.Name).MaximumLength(50);
        }
    }

    internal class AddInsuranceTypeCommandHandler : ICommandHandler<AddInsuranceTypeCommand, IResponse<InsureAnts.Domain.Entities.InsuranceType>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddInsuranceTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.InsuranceType>> Handle(AddInsuranceTypeCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<InsureAnts.Domain.Entities.InsuranceType>(command);

            _unitOfWork.InsuranceTypes.Add(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Created<InsureAnts.Domain.Entities.InsuranceType>($"for feed {command.Id}")).For(entity);
        }
    }
}
