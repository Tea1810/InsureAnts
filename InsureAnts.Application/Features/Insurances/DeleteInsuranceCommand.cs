using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.Features.Insurances;

public class DeleteInsuranceCommand : DeleteCommand<int>;
internal class DeleteInsuranceCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeleteInsuranceCommand, Insurance, int>(unitOfWork)
{
    protected override IRepository<Insurance, int> GetRepository() => UnitOfWork.Insurances;
}
