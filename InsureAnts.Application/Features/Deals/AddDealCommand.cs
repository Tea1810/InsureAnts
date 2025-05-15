using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Deals;

public class AddDealCommand : ICommand<IResponse<Deal>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationInDays { get; set; }
    public double DiscountPercentage { get; set; }
}

[UsedImplicitly]
internal class AddDealCommandValidator : AbstractValidator<Deal>
{
    public AddDealCommandValidator()
    {
        RuleFor(command => command.Name).MaximumLength(50);
        RuleFor(command => command.Description).MaximumLength(200);
    }
}

internal class AddDealCommandHandler : ICommandHandler<AddDealCommand, IResponse<Deal>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddDealCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Deal>> Handle(AddDealCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Deal>(command);

        _unitOfWork.Deals.Add(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Created<Client>(entity.Name)).For(entity);
    }
}
