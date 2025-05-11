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

namespace InsureAnts.Application.Features.Deal
{
    public class AddDealCommand : ICommand<IResponse<InsureAnts.Domain.Entities.Deal>>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int DurationInDays { get; set; }
        public double DiscountPercentage { get; set; }

        public List<ClientPackage>? ClientPackages { get; set; }
        public List<InsureAnts.Domain.Entities.Client>? Clients { get; set; }
    }

    [UsedImplicitly]
    internal class AddDealCommandValidator : AbstractValidator<InsureAnts.Domain.Entities.Deal>
    {
        public AddDealCommandValidator()
        {
            RuleFor(command => command.Name).MaximumLength(50);
            RuleFor(command => command.Description).MaximumLength(200);
        }
    }

    internal class AddDealCommandHandler : ICommandHandler<AddDealCommand, IResponse<InsureAnts.Domain.Entities.Deal>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddDealCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.Deal>> Handle(AddDealCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<InsureAnts.Domain.Entities.Deal>(command);

            _unitOfWork.Deals.Add(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Created<InsureAnts.Domain.Entities.Client>($"for feed {command.Id}")).For(entity);
        }
    }
}
