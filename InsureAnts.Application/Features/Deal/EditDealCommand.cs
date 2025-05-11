using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Deal
{
    public class EditDealCommand : EditCommand<InsureAnts.Domain.Entities.Deal, InsureAnts.Domain.Entities.Deal, int>
    {
        public bool WasSeen { get; set; }
    }

    [UsedImplicitly]
    internal class EditDealCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditDealCommand, InsureAnts.Domain.Entities.Deal, InsureAnts.Domain.Entities.Deal, int>(unitOfWork)
    {
        protected override IQueryable<InsureAnts.Domain.Entities.Deal> GetTrackedQuery() => UnitOfWork.Deals.AllTracked();
    }

    internal class EditDealCommandHandler : ICommandHandler<EditDealCommand, IResponse<InsureAnts.Domain.Entities.Deal>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditDealCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.Deal>> Handle(EditDealCommand command, CancellationToken cancellationToken)
        {
            var client = command.Entity!;

            var entity = _mapper.Map(command, client);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Updated<InsureAnts.Domain.Entities.Deal>(entity.Id.ToString())).For(entity);
        }
    }
}
