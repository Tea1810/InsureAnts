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

namespace InsureAnts.Application.Features.Insurance
{
    public class AddInsuranceCommand : ICommand<IResponse<InsureAnts.Domain.Entities.Insurance>>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public double Premium { get; set; }
        public double Coverage { get; set; }
        public int DurationInDays { get; set; }
        public AvailabilityStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int InsuranceTypeId { get; set; }

        public InsuranceType? InsuranceType { get; set; }
        public List<Package>? Packages { get; set; }
        public List<InsureAnts.Domain.Entities.Client>? Clients { get; set; }
    }

    [UsedImplicitly]
    internal class AddInsuranceCommandValidator : AbstractValidator<InsureAnts.Domain.Entities.Insurance>
    {
        public AddInsuranceCommandValidator()
        {
            RuleFor(command => command.Name).MaximumLength(50);
            RuleFor(command => command.Description).MaximumLength(200);
        }
    }

    internal class AddInsuranceCommandHandler : ICommandHandler<AddInsuranceCommand, IResponse<InsureAnts.Domain.Entities.Insurance>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddInsuranceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.Insurance>> Handle(AddInsuranceCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<InsureAnts.Domain.Entities.Insurance>(command);

            _unitOfWork.Insurances.Add(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Created<InsureAnts.Domain.Entities.Insurance>($"for feed {command.Id}")).For(entity);
        }
    }
}
