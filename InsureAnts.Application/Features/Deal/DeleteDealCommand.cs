using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;

namespace InsureAnts.Application.Features.Deal
{
    public class DeleteDealCommand : DeleteCommand<int>;
    internal class DeleteDealCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeleteDealCommand, InsureAnts.Domain.Entities.Deal, int>(unitOfWork)
    {
        protected override IRepository<InsureAnts.Domain.Entities.Deal, int> GetRepository() => UnitOfWork.Deals;
    }
}
