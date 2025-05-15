using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Deals;

public class EditDealCommand : EditCommand<Deal, Deal, int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationInDays { get; set; }
    public double DiscountPercentage { get; set; }
}

[UsedImplicitly]
internal class EditDealCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditDealCommand, Deal, Deal, int>(unitOfWork)
{
    protected override IQueryable<Deal> GetTrackedQuery() => UnitOfWork.Deals.AllTracked();
}

[UsedImplicitly]
internal class EditDealCommandValidator : AbstractValidator<Deal>
{
    public EditDealCommandValidator()
    {
        RuleFor(command => command.Name).MaximumLength(100).NotEmpty();
        RuleFor(command => command.Description).MaximumLength(500).NotEmpty();
        RuleFor(command => command.DurationInDays).GreaterThanOrEqualTo(1);
        RuleFor(command => command.DiscountPercentage).GreaterThanOrEqualTo(1);
    }
}

internal class EditDealCommandHandler : ICommandHandler<EditDealCommand, IResponse<Deal>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditDealCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Deal>> Handle(EditDealCommand command, CancellationToken cancellationToken)
    {
        var client = command.Entity!;

        var entity = _mapper.Map(command, client);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Updated<Deal>(entity.Id.ToString())).For(entity);
    }
}
