using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;

namespace InsureAnts.Application.Features.Insurance
{
    public class DeleteInsuranceCommand : DeleteCommand<int>;
    internal class DeleteInsuranceCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeleteInsuranceCommand, InsureAnts.Domain.Entities.Insurance, int>(unitOfWork)
    {
        protected override IRepository<InsureAnts.Domain.Entities.Insurance, int> GetRepository() => UnitOfWork.Insurances;
    }
}
