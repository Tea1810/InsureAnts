using AutoMapper;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Insurance
{
    public class EditInsuranceCommand : EditCommand<InsureAnts.Domain.Entities.Insurance, InsureAnts.Domain.Entities.Insurance, int>
    {
        public bool WasSeen { get; set; }
    }

    [UsedImplicitly]
    internal class EditInsuranceCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditInsuranceCommand, InsureAnts.Domain.Entities.Insurance, InsureAnts.Domain.Entities.Insurance, int>(unitOfWork)
    {
        protected override IQueryable<InsureAnts.Domain.Entities.Insurance> GetTrackedQuery() => UnitOfWork.Insurances.AllTracked();
    }

    internal class EditInsuranceCommandHandler : ICommandHandler<EditInsuranceCommand, IResponse<InsureAnts.Domain.Entities.Insurance>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditInsuranceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.Insurance>> Handle(EditInsuranceCommand command, CancellationToken cancellationToken)
        {
            var client = command.Entity!;

            var entity = _mapper.Map(command, client);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Updated<InsureAnts.Domain.Entities.Insurance>(entity.Id.ToString())).For(entity);
        }
    }
}
