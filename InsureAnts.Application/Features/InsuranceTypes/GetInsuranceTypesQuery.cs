using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Common;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.Features.InsuranceTypes;

public class GetInsuranceTypesQuery : AbstractQueryRequest<InsuranceType>
{
    public string SearchTerm { get; set; } = string.Empty;

    public override IQueryable<InsuranceType> ApplyFilter(IQueryable<InsuranceType> source)
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            source = source.Where(c => c.Name.Contains(SearchTerm));
        }


        return base.ApplyFilter(source);
    }
}

internal class GetInsuranceTypesQueryHandler : IQueryHandler<GetInsuranceTypesQuery, QueryResult<InsuranceType>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInsuranceTypesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ValueTask<QueryResult<InsuranceType>> Handle(GetInsuranceTypesQuery query, CancellationToken cancellationToken)
    {
        return _unitOfWork.InsuranceTypes.All().GetResultAsync(query, cancellationToken).ToValueTask();
    }
}

