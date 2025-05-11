using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;

namespace InsureAnts.Application.Features.Package
{
    public class DeletePackageCommand : DeleteCommand<int>;
    internal class DeletePackageCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeletePackageCommand, InsureAnts.Domain.Entities.Package, int>(unitOfWork)
    {
        protected override IRepository<InsureAnts.Domain.Entities.Package, int> GetRepository() => UnitOfWork.Packages;
    }
}
