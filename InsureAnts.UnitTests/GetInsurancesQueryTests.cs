using System.Collections.Generic;
using System.Linq;
using InsureAnts.Application.Features.Insurances;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using Xunit;

namespace InsureAnts.UnitTests
{
    public class GetInsurancesQueryTests
    {
        private List<Insurance> GetMockInsurances() => new()
    {
        new Insurance { Name = "Life Cover", Status = AvailabilityStatus.Active, InsuranceType = new InsuranceType { Name = "Life" } },
        new Insurance { Name = "Health Shield", Status = AvailabilityStatus.Inactive, InsuranceType = new InsuranceType { Name = "Health" } },
        new Insurance { Name = "Car Protect", Status = AvailabilityStatus.Active, InsuranceType = new InsuranceType { Name = "Vehicle" } },
        new Insurance { Name = "Dental Plan", Status = AvailabilityStatus.Inactive, InsuranceType = new InsuranceType { Name = "Health" } }
    };

        [Fact]
        public void ApplyFilter_Should_Filter_By_Name()
        {
            var query = new GetInsurancesQuery { SearchTerm = "Life" };
            var insurances = GetMockInsurances().AsQueryable();

            var result = query.ApplyFilter(insurances).ToList();

            Assert.Single(result);
            Assert.Equal("Life Cover", result[0].Name);
        }

        [Fact]
        public void ApplyFilter_Should_Filter_By_InsuranceType()
        {
            var query = new GetInsurancesQuery { InsuranceTypeFilter = "Health" };
            var insurances = GetMockInsurances().AsQueryable();

            var result = query.ApplyFilter(insurances).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, i => Assert.Equal("Health", i.InsuranceType.Name));
        }

        [Fact]
        public void ApplyFilter_Should_Filter_By_Status_Active()
        {
            var query = new GetInsurancesQuery { StatusFilter = AvailabilityStatusFilter.Active };
            var insurances = GetMockInsurances().AsQueryable();

            var result = query.ApplyFilter(insurances).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, i => Assert.Equal(AvailabilityStatus.Active, i.Status));
        }

        [Fact]
        public void ApplyFilter_Should_Filter_By_Status_Inactive()
        {
            var query = new GetInsurancesQuery { StatusFilter = AvailabilityStatusFilter.Inactive };
            var insurances = GetMockInsurances().AsQueryable();

            var result = query.ApplyFilter(insurances).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, i => Assert.Equal(AvailabilityStatus.Inactive, i.Status));
        }

        [Fact]
        public void ApplyFilter_Should_Filter_By_All_Criteria()
        {
            var query = new GetInsurancesQuery
            {
                SearchTerm = "Plan",
                InsuranceTypeFilter = "Health",
                StatusFilter = AvailabilityStatusFilter.Inactive
            };
            var insurances = GetMockInsurances().AsQueryable();

            var result = query.ApplyFilter(insurances).ToList();

            Assert.Single(result);
            Assert.Equal("Dental Plan", result[0].Name);
        }

        [Fact]
        public void ApplyFilter_Should_Return_All_When_No_Filters_Applied()
        {
            var query = new GetInsurancesQuery();
            var insurances = GetMockInsurances().AsQueryable();

            var result = query.ApplyFilter(insurances).ToList();

            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void ApplyFilter_Should_Return_None_When_No_Match()
        {
            var query = new GetInsurancesQuery
            {
                SearchTerm = "Unknown",
                InsuranceTypeFilter = "Nonexistent",
                StatusFilter = AvailabilityStatusFilter.Active
            };
            var insurances = GetMockInsurances().AsQueryable();

            var result = query.ApplyFilter(insurances).ToList();

            Assert.Empty(result);
        }
    }
}