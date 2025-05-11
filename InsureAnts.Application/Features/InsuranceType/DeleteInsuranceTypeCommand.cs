using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;

namespace InsureAnts.Application.Features.InsuranceType
{
    public class DeleteInsuranceTypeCommand : DeleteCommand<int>;
    internal class DeleteInsuranceTypeCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeleteInsuranceTypeCommand, InsureAnts.Domain.Entities.InsuranceType, int>(unitOfWork)
    {
        protected override IRepository<InsureAnts.Domain.Entities.InsuranceType, int> GetRepository() => UnitOfWork.InsuranceTypes;
    }
}
