using InsureAnts.Application.Features.Clients;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;

namespace InsureAnts.UnitTests;

public class GetClientsQueryTests
{
    private List<Client> GetMockClients() => new()
{
    new Client { FirstName = "Alice", LastName = "Smith", Status = AvailabilityStatus.Active, Gender = Gender.Female },
    new Client { FirstName = "Bob", LastName = "Johnson", Status = AvailabilityStatus.Inactive, Gender = Gender.Male },
    new Client { FirstName = "Charlie", LastName = "Brown", Status = AvailabilityStatus.Active, Gender = Gender.Male },
    new Client { FirstName = "Diana", LastName = "Prince", Status = AvailabilityStatus.Inactive, Gender = Gender.Female }
};

    [Fact]
    public void ApplyFilter_Should_Filter_By_SearchTerm()
    {
        // Arrange
        var query = new GetClientsQuery { SearchTerm = "Alice" };
        var clients = GetMockClients().AsQueryable();

        // Act
        var result = query.ApplyFilter(clients).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Alice", result[0].FirstName);
    }

    [Fact]
    public void ApplyFilter_Should_Filter_By_Status()
    {
        // Arrange
        var query = new GetClientsQuery { StatusFilter = AvailabilityStatusFilter.Active };
        var clients = GetMockClients().AsQueryable();

        // Act
        var result = query.ApplyFilter(clients).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal(AvailabilityStatus.Active, c.Status));
    }

    [Fact]
    public void ApplyFilter_Should_Filter_By_Gender()
    {
        // Arrange
        var query = new GetClientsQuery { GenderFilter = GenderFilter.Female };
        var clients = GetMockClients().AsQueryable();

        // Act
        var result = query.ApplyFilter(clients).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal(Gender.Female, c.Gender));
    }

    [Fact]
    public void ApplyFilter_Should_Apply_All_Filters_Together()
    {
        // Arrange
        var query = new GetClientsQuery
        {
            SearchTerm = "Diana",
            StatusFilter = AvailabilityStatusFilter.Inactive,
            GenderFilter = GenderFilter.Female
        };

        var clients = GetMockClients().AsQueryable();

        // Act
        var result = query.ApplyFilter(clients).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Diana", result[0].FirstName);
    }

    [Fact]
    public void ApplyFilter_Should_Return_All_When_No_Filters()
    {
        // Arrange
        var query = new GetClientsQuery(); // All defaults
        var clients = GetMockClients().AsQueryable();

        // Act
        var result = query.ApplyFilter(clients).ToList();

        // Assert
        Assert.Equal(4, result.Count);
    }
}