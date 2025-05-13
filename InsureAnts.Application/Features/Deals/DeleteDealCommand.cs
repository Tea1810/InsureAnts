using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.Features.Deals;

public class DeleteDealCommand : DeleteCommand<int>;
internal class DeleteDealCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeleteDealCommand, Deal, int>(unitOfWork)
{
    protected override IRepository<Deal, int> GetRepository() => UnitOfWork.Deals;
}
