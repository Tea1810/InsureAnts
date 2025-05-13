using AutoMapper;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Packs;

public class EditPackageCommand : EditCommand<Package, Package, int>
{
    public bool WasSeen { get; set; }
}

[UsedImplicitly]
internal class EditPackageCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditPackageCommand, Package, Package, int>(unitOfWork)
{
    protected override IQueryable<Package> GetTrackedQuery() => UnitOfWork.Packages.AllTracked();
}

internal class EditPackageCommandHandler : ICommandHandler<EditPackageCommand, IResponse<Package>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditPackageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Package>> Handle(EditPackageCommand command, CancellationToken cancellationToken)
    {
        var package = command.Entity!;

        var entity = _mapper.Map(command, package);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Updated<Package>(entity.Id.ToString())).For(entity);
    }
}
