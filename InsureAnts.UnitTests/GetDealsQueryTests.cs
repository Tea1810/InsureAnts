using InsureAnts.Application.Features.Deals;
using InsureAnts.Domain.Entities;

namespace InsureAnts.UnitTests
{
    public class GetDealsQueryTests
    {
        private List<Deal> GetMockDeals() => new()
    {
        new Deal { Name = "Life Insurance", Description = "Full life coverage deal" },
        new Deal { Name = "Health Plan", Description = "Comprehensive health insurance" },
        new Deal { Name = "Dental Cover", Description = "Covers dental procedures" },
        new Deal { Name = "Vision Plan", Description = "Coverage for eye care" }
    };

        [Fact]
        public void ApplyFilter_Should_Filter_By_Name()
        {
            // Arrange
            var query = new GetDealsQuery { SearchTerm = "Life" };
            var deals = GetMockDeals().AsQueryable();

            // Act
            var result = query.ApplyFilter(deals).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Life Insurance", result[0].Name);
        }

        [Fact]
        public void ApplyFilter_Should_Filter_By_Description()
        {
            // Arrange
            var query = new GetDealsQuery { SearchTerm = "dental" };
            var deals = GetMockDeals().AsQueryable();

            // Act
            var result = query.ApplyFilter(deals).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Dental Cover", result[0].Name);
        }

        [Fact]
        public void ApplyFilter_Should_Return_Multiple_Results()
        {
            // Arrange
            var query = new GetDealsQuery { SearchTerm = "Plan" };
            var deals = GetMockDeals().AsQueryable();

            // Act
            var result = query.ApplyFilter(deals).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, d => d.Name == "Health Plan");
            Assert.Contains(result, d => d.Name == "Vision Plan");
        }

        [Fact]
        public void ApplyFilter_Should_Return_All_When_SearchTerm_Empty()
        {
            // Arrange
            var query = new GetDealsQuery { SearchTerm = string.Empty };
            var deals = GetMockDeals().AsQueryable();

            // Act
            var result = query.ApplyFilter(deals).ToList();

            // Assert
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void ApplyFilter_Should_Return_None_When_No_Match()
        {
            // Arrange
            var query = new GetDealsQuery { SearchTerm = "Car Insurance" };
            var deals = GetMockDeals().AsQueryable();

            // Act
            var result = query.ApplyFilter(deals).ToList();

            // Assert
            Assert.Empty(result);
        }
    }
}