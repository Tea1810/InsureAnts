using System.Reflection.Metadata;
using AutoMapper;
using FluentAssertions;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Insurances;
using InsureAnts.Application.Features.InsuranceTypes;
using InsureAnts.Application.Features.Packs;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using Moq;

namespace InsureAnts.UnitTests;

public class AddCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IMapper> _mockMapper = new();

    private readonly AddInsuranceCommandHandler _insurancesHandler;
    private readonly AddInsuranceTypeCommandHandler _insuranceTypesHandler;
    private readonly AddPackageCommandHandler _packagesHandler;

    public AddCommandHandlerTests()
    {
        _insurancesHandler = new AddInsuranceCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        _insuranceTypesHandler = new AddInsuranceTypeCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        _packagesHandler = new AddPackageCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddInsurance_WhenCommandIsValid()
    {
        // Arrange
        var command = new AddInsuranceCommand
        {
            Name = "Life Cover",
            Description = "Full life coverage",
            Premium = 100.0,
            Coverage = 100000.0,
            DurationInDays = 365,
            Status = AvailabilityStatus.Active,
            CreatedAt = DateTime.UtcNow,
            InsuranceType = new InsuranceType { Id = 1, Name = "Life" }
        };

        var mappedInsurance = new Insurance
        {
            Name = command.Name,
            Description = command.Description,
            Premium = command.Premium,
            Coverage = command.Coverage,
            DurationInDays = command.DurationInDays,
            Status = command.Status,
            CreatedAt = command.CreatedAt,
            InsuranceType = command.InsuranceType
        };

        _mockMapper.Setup(m => m.Map<Insurance>(command)).Returns(mappedInsurance);

        var mockInsuranceRepo = new Mock<IRepository<Insurance, int>>();
        var mockInsuranceTypeRepo = new Mock<IRepository<InsuranceType, int>>();

        _mockUnitOfWork.SetupGet(u => u.Insurances).Returns(mockInsuranceRepo.Object);
        _mockUnitOfWork.SetupGet(u => u.InsuranceTypes).Returns(mockInsuranceTypeRepo.Object);

        // Act
        var result = await _insurancesHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(mappedInsurance);

        mockInsuranceTypeRepo.Verify(r => r.Track(mappedInsurance.InsuranceType!), Times.Once);
        mockInsuranceRepo.Verify(r => r.Add(mappedInsurance), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldAddInsuranceType_WhenCommandIsValid()
    {
        // Arrange
        var command = new AddInsuranceTypeCommand
        {
            Name = "Vehicle"
        };

        var mappedInsuranceType = new InsuranceType
        {
            Name = command.Name
        };

        // Mock mapping
        _mockMapper.Setup(m => m.Map<InsuranceType>(command)).Returns(mappedInsuranceType);

        // Mock repository
        var mockInsuranceTypeRepo = new Mock<IRepository<InsuranceType, int>>();
        _mockUnitOfWork.SetupGet(u => u.InsuranceTypes).Returns(mockInsuranceTypeRepo.Object);

        // Act
        var result = await _insuranceTypesHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(mappedInsuranceType);

        mockInsuranceTypeRepo.Verify(r => r.Add(mappedInsuranceType), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldAddPackageAndTrackInsurances_WhenCommandIsValid()
    {
        // Arrange
        var insurance1 = new Insurance { Id = 25, Name = "Health", Description="Test" };
        var insurance2 = new Insurance { Id = 36, Name = "Car", Description = "Test" };

        var command = new AddPackageCommand
        {
            Name = "Family Pack",
            Description = "A bundle of insurances",
            Premium = 100.0,
            DurationInDays = 365,
            Status = AvailabilityStatus.Active,
            Insurances = new[] { insurance1, insurance2 }
        };

        var mappedPackage = new Package
        {
            Id = 44,
            Name = command.Name,
            Description = command.Description,
            Premium = command.Premium,
            DurationInDays = command.DurationInDays,
            Status = command.Status,
            Insurances = new List<Insurance> { insurance1, insurance2 }
        };

        // Mock mapping
        _mockMapper.Setup(m => m.Map<Package>(command)).Returns(mappedPackage);

        // Mock repository access
        var mockInsuranceRepo = new Mock<IRepository<Insurance, int>>();
        var mockPackageRepo = new Mock<IRepository<Package, int>>();
        _mockUnitOfWork.SetupGet(u => u.Insurances).Returns(mockInsuranceRepo.Object);
        _mockUnitOfWork.SetupGet(u => u.Packages).Returns(mockPackageRepo.Object);

        // Act
        var result = await _packagesHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(mappedPackage);

        mockInsuranceRepo.Verify(r => r.Track(insurance1), Times.Once);
        mockInsuranceRepo.Verify(r => r.Track(insurance2), Times.Once);
        mockPackageRepo.Verify(r => r.Add(mappedPackage), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}