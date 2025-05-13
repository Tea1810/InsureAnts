using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.Features.Clients;

public class DeleteClientCommand : DeleteCommand<int>;
internal class DeleteClientCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeleteClientCommand, Client, int>(unitOfWork)
{
    protected override IRepository<Client, int> GetRepository() => UnitOfWork.Clients;
}
