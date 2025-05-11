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

namespace InsureAnts.Application.Features.Package
{
    public class AddDealCommand : ICommand<IResponse<InsureAnts.Domain.Entities.Package>>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public double Premium { get; set; }
        public AvailabilityStatus Status { get; set; }
        public int DurationInDays { get; set; }

        public List<InsureAnts.Domain.Entities.Insurance>? Insurances { get; set; }

        public List<ClientPackage>? ClientPackages { get; set; }
    }

    [UsedImplicitly]
    internal class AddPackageCommandValidator : AbstractValidator<InsureAnts.Domain.Entities.Package>
    {
        public AddPackageCommandValidator()
        {
            RuleFor(command => command.Name).MaximumLength(50);
            RuleFor(command => command.Description).MaximumLength(200);
            RuleFor(command => command.Status).IsInEnum();
        }
    }

    internal class AddPackageCommandHandler : ICommandHandler<AddDealCommand, IResponse<InsureAnts.Domain.Entities.Package>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddPackageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.Package>> Handle(AddDealCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<InsureAnts.Domain.Entities.Package>(command);

            _unitOfWork.Packages.Add(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Created<InsureAnts.Domain.Entities.Package>($"for feed {command.Id}")).For(entity);
        }
    }
}
