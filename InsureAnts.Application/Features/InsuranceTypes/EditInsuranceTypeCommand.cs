using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.InsuranceTypes
{
    public class EditInsuranceTypeCommand : EditCommand<InsuranceType, InsuranceType, int>
    {
        public string Name { get; set; } = string.Empty;
    }

    [UsedImplicitly]
    internal class EditInsuranceTypeCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditInsuranceTypeCommand, InsuranceType, InsuranceType, int>(unitOfWork)
    {
        protected override IQueryable<InsuranceType> GetTrackedQuery() => UnitOfWork.InsuranceTypes.AllTracked();
    }


    [UsedImplicitly]
    internal class EditInsuranceTypeCommandValidator : AbstractValidator<InsuranceType>
    {
        public EditInsuranceTypeCommandValidator()
        {
            RuleFor(command => command.Name).MaximumLength(100).NotEmpty() ;
        }
    }
    internal class EditInsuranceTypeCommandHandler : ICommandHandler<EditInsuranceTypeCommand, IResponse<InsuranceType>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditInsuranceTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsuranceType>> Handle(EditInsuranceTypeCommand command, CancellationToken cancellationToken)
        {
            var insuranceType = command.Entity!;

            var entity = _mapper.Map(command, insuranceType);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Updated<InsuranceType>(entity.Id.ToString())).For(entity);
        }
    }
}
