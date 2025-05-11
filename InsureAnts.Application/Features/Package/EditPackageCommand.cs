using AutoMapper;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Package
{
    public class EditPackageCommand : EditCommand<InsureAnts.Domain.Entities.Package, InsureAnts.Domain.Entities.Package, int>
    {
        public bool WasSeen { get; set; }
    }

    [UsedImplicitly]
    internal class EditPackageCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditPackageCommand, InsureAnts.Domain.Entities.Package, InsureAnts.Domain.Entities.Package, int>(unitOfWork)
    {
        protected override IQueryable<InsureAnts.Domain.Entities.Package> GetTrackedQuery() => UnitOfWork.Packages.AllTracked();
    }

    internal class EditPackageCommandHandler : ICommandHandler<EditPackageCommand, IResponse<InsureAnts.Domain.Entities.Package>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditPackageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.Package>> Handle(EditPackageCommand command, CancellationToken cancellationToken)
        {
            var package = command.Entity!;

            var entity = _mapper.Map(command, package);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Updated<InsureAnts.Domain.Entities.Package>(entity.Id.ToString())).For(entity);
        }
    }
}
