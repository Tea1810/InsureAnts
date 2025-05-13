using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.Features.Packs;

public class DeletePackageCommand : DeleteCommand<int>;
internal class DeletePackageCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeletePackageCommand, Package, int>(unitOfWork)
{
    protected override IRepository<Package, int> GetRepository() => UnitOfWork.Packages;
}
