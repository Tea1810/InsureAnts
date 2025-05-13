using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.Features.InsuranceTypes
{
    public class DeleteInsuranceTypeCommand : DeleteCommand<int>;
    internal class DeleteInsuranceTypeCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeleteInsuranceTypeCommand, InsuranceType, int>(unitOfWork)
    {
        protected override IRepository<InsuranceType, int> GetRepository() => UnitOfWork.InsuranceTypes;
    }
}
