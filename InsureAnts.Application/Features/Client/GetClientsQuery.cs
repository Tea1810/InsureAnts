using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.DataAccess.Interfaces;

namespace InsureAnts.Application.Features.Client
{
    public class GetFeedAlertsQuery : AbstractQueryRequest<InsureAnts.Domain.Entities.Client>
    {
        public string SearchTerm { get; set; } = string.Empty;

        public AlertTypeEnum TypeFilter { get; set; } = AlertTypeEnum.All;

        public AlertStatusEnum StatusFilter { get; set; } = AlertStatusEnum.All;

        public int? Id { get; set; } = null;

        public override IQueryable<InsureAnts.Domain.Entities.Client> ApplyFilter(IQueryable<InsureAnts.Domain.Entities.Client> source)
        {
            if (Id != null)
            {
                source = source.Where(fa => fa.Id == Id);
            }

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                source = source.Where(fa => fa.WebShopFeed.Name.Contains(SearchTerm));
            }

            source = TypeFilter switch
            {
                AlertTypeEnum.Critical => source.Where(fa => fa.AlertType == AlertType.Critical),
                AlertTypeEnum.NonCritical => source.Where(fa => fa.AlertType == AlertType.NonCritical),
                AlertTypeEnum.StorageWarnings => source.Where(fa => fa.AlertType == AlertType.StorageWarnings),
                _ => source
            };

            source = StatusFilter switch
            {
                AlertStatusEnum.New => source.Where(fa => fa.WasSeen == false),
                AlertStatusEnum.Old => source.Where(fa => fa.WasSeen == true),
                _ => source
            };

            return base.ApplyFilter(source);
        }
    }

    internal class GetFeedAlertsQueryHandler : IQueryHandler<GetFeedAlertsQuery, QueryResult<InsureAnts.Domain.Entities.Client>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFeedAlertsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ValueTask<QueryResult<InsureAnts.Domain.Entities.Client>> Handle(GetFeedAlertsQuery query, CancellationToken cancellationToken)
        {
            return _unitOfWork.Clients.All().Include(fa => fa.WebShopFeed.WebShop).GetResultAsync(query, cancellationToken).ToValueTask();
        }
    }

}
