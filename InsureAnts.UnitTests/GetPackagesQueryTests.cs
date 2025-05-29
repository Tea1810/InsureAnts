using InsureAnts.Application.Features.Packages;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;

namespace InsureAnts.UnitTests
{
    public class GetPackagesQueryTests
    {
        private List<Package> GetMockPackages() => new()
    {
        new Package
        {
            Name = "Starter Pack",
            Status = AvailabilityStatus.Active,
            Insurances = new List<Insurance>
            {
                new Insurance { Name = "Life Cover" },
                new Insurance { Name = "Dental" }
            }
        },
        new Package
        {
            Name = "Family Plan",
            Status = AvailabilityStatus.Inactive,
            Insurances = new List<Insurance>
            {
                new Insurance { Name = "Health Shield" },
                new Insurance { Name = "Dental" }
            }
        },
        new Package
        {
            Name = "Vehicle Bundle",
            Status = AvailabilityStatus.Active,
            Insurances = new List<Insurance>
            {
                new Insurance { Name = "Car Protect" }
            }
        }
    };

        [Fact]
        public void ApplyFilter_Should_Filter_By_SearchTerm()
        {
            var query = new GetPackagesQuery { SearchTerm = "Family" };
            var packages = GetMockPackages().AsQueryable();

            var result = query.ApplyFilter(packages).ToList();

            Assert.Single(result);
            Assert.Equal("Family Plan", result[0].Name);
        }

        [Fact]
        public void ApplyFilter_Should_Filter_By_Insurance_Name()
        {
            var query = new GetPackagesQuery { InsuranceFilter = "Dental" };
            var packages = GetMockPackages().AsQueryable();

            var result = query.ApplyFilter(packages).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Contains(p.Insurances, i => i.Name == "Dental"));
        }

        [Fact]
        public void ApplyFilter_Should_Filter_By_Status_Active()
        {
            var query = new GetPackagesQuery { StatusFilter = AvailabilityStatusFilter.Active };
            var packages = GetMockPackages().AsQueryable();

            var result = query.ApplyFilter(packages).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Equal(AvailabilityStatus.Active, p.Status));
        }

        [Fact]
        public void ApplyFilter_Should_Filter_By_Status_Inactive()
        {
            var query = new GetPackagesQuery { StatusFilter = AvailabilityStatusFilter.Inactive };
            var packages = GetMockPackages().AsQueryable();

            var result = query.ApplyFilter(packages).ToList();

            Assert.Single(result);
            Assert.Equal("Family Plan", result[0].Name);
        }

        [Fact]
        public void ApplyFilter_Should_Filter_By_All_Criteria()
        {
            var query = new GetPackagesQuery
            {
                SearchTerm = "Family",
                InsuranceFilter = "Dental",
                StatusFilter = AvailabilityStatusFilter.Inactive
            };
            var packages = GetMockPackages().AsQueryable();

            var result = query.ApplyFilter(packages).ToList();

            Assert.Single(result);
            Assert.Equal("Family Plan", result[0].Name);
        }

        [Fact]
        public void ApplyFilter_Should_Return_All_When_No_Filters()
        {
            var query = new GetPackagesQuery();
            var packages = GetMockPackages().AsQueryable();

            var result = query.ApplyFilter(packages).ToList();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void ApplyFilter_Should_Return_Empty_When_No_Match()
        {
            var query = new GetPackagesQuery
            {
                SearchTerm = "Unknown",
                InsuranceFilter = "Nonexistent",
                StatusFilter = AvailabilityStatusFilter.Active
            };
            var packages = GetMockPackages().AsQueryable();

            var result = query.ApplyFilter(packages).ToList();

            Assert.Empty(result);
        }
    }
}