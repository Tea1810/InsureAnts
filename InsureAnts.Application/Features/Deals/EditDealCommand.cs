using AutoMapper;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Deals;

public class EditDealCommand : EditCommand<Deal, Deal, int>
{
    public bool WasSeen { get; set; }
}

[UsedImplicitly]
internal class EditDealCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditDealCommand, Deal, Deal, int>(unitOfWork)
{
    protected override IQueryable<Deal> GetTrackedQuery() => UnitOfWork.Deals.AllTracked();
}

internal class EditDealCommandHandler : ICommandHandler<EditDealCommand, IResponse<Deal>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditDealCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Deal>> Handle(EditDealCommand command, CancellationToken cancellationToken)
    {
        var client = command.Entity!;

        var entity = _mapper.Map(command, client);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Updated<Deal>(entity.Id.ToString())).For(entity);
    }
}
