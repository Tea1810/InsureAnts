using AutoMapper;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Clients;

public class EditClientCommand : EditCommand<Client, Client, int>
{
    public bool WasSeen { get; set; }
}

[UsedImplicitly]
internal class EditClientCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditClientCommand, Client, Client, int>(unitOfWork)
{
    protected override IQueryable<Domain.Entities.Client> GetTrackedQuery() => UnitOfWork.Clients.AllTracked();
}

internal class EditClientCommandHandler : ICommandHandler<EditClientCommand, IResponse<Client>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Domain.Entities.Client>> Handle(EditClientCommand command, CancellationToken cancellationToken)
    {
        var client = command.Entity!;

        var entity = _mapper.Map(command, client);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Updated<Client>(entity.Id.ToString())).For(entity);
    }
}
