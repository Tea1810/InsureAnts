using AutoMapper;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Client
{
    public class EditClientCommand : EditCommand<InsureAnts.Domain.Entities.Client, InsureAnts.Domain.Entities.Client, int>
    {
        public bool WasSeen { get; set; }
    }

    [UsedImplicitly]
    internal class EditClientCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditClientCommand, InsureAnts.Domain.Entities.Client, InsureAnts.Domain.Entities.Client, int>(unitOfWork)
    {
        protected override IQueryable<InsureAnts.Domain.Entities.Client> GetTrackedQuery() => UnitOfWork.Clients.AllTracked();
    }

    internal class EditClientCommandHandler : ICommandHandler<EditClientCommand, IResponse<InsureAnts.Domain.Entities.Client>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.Client>> Handle(EditClientCommand command, CancellationToken cancellationToken)
        {
            var client = command.Entity!;

            var entity = _mapper.Map(command, client);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Updated<InsureAnts.Domain.Entities.Client>(entity.Id.ToString())).For(entity);
        }
    }
}
