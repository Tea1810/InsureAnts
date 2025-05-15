using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.InsuranceTypes;

public class AddInsuranceTypeCommand : ICommand<IResponse<InsuranceType>>
{
    public string Name { get; set; } = string.Empty;
}

[UsedImplicitly]
internal class AddInsuranceTypeCommandValidator : AbstractValidator<InsuranceType>
{
    public AddInsuranceTypeCommandValidator()
    {
        RuleFor(command => command.Name).MaximumLength(100);
    }
}

internal class AddInsuranceTypeCommandHandler : ICommandHandler<AddInsuranceTypeCommand, IResponse<InsuranceType>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddInsuranceTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<InsuranceType>> Handle(AddInsuranceTypeCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<InsuranceType>(command);

        _unitOfWork.InsuranceTypes.Add(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Created<InsuranceType>(command.Name)).For(entity);
    }
}
