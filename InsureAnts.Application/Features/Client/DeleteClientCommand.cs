using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;

namespace InsureAnts.Application.Features.Client
{
    public class DeleteClientCommand : DeleteCommand<int>;
    internal class DeleteClientCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeleteClientCommand, InsureAnts.Domain.Entities.Client, int>(unitOfWork)
    {
        protected override IRepository<InsureAnts.Domain.Entities.Client, int> GetRepository() => UnitOfWork.Clients;
    }
}
