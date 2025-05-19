using System.Reflection.Metadata;
using AutoMapper;
using FluentAssertions;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Clients;
using InsureAnts.Application.Features.Deals;
using InsureAnts.Application.Features.Insurances;
using InsureAnts.Application.Features.InsuranceTypes;
using InsureAnts.Application.Features.Packs;
using InsureAnts.Application.Features.SupportTickets;
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
    private readonly AddDealCommandHandler _dealsHandler;
    private readonly AddSupportTicketCommandHandler _ticketsHandler;
    private readonly AddClientCommandHandler _clientsHandler;

    public AddCommandHandlerTests()
    {
        _insurancesHandler = new AddInsuranceCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        _insuranceTypesHandler = new AddInsuranceTypeCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        _packagesHandler = new AddPackageCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        _dealsHandler = new AddDealCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        _ticketsHandler = new AddSupportTicketCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        _clientsHandler = new AddClientCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
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

    [Fact]
    public async Task Handle_ShouldAddDeal_WhenCommandIsValid()
    {
        // Arrange
        var command = new AddDealCommand
        {
            Name = "Spring Promo",
            Description = "20% off all packages",
            DurationInDays = 30,
            DiscountPercentage = 20.0
        };
        var mapped = new Deal
        {
            Name = command.Name,
            Description = command.Description,
            DurationInDays = command.DurationInDays,
            DiscountPercentage = command.DiscountPercentage
        };
        _mockMapper.Setup(m => m.Map<Deal>(command)).Returns(mapped);

        var mockRepo = new Mock<IRepository<Deal, int>>();
        _mockUnitOfWork.SetupGet(x => x.Deals).Returns(mockRepo.Object);

        // Act
        var result = await _dealsHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(mapped);

        mockRepo.Verify(r => r.Add(mapped), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldAddSupportTicket_WhenCommandIsValid()
    {
        // Arrange
        var client = new Client { Id = 7, FirstName = "Jane", LastName = "Doe", Email="nef", Phone="0837472810", Password="ErnwebwER#$GT#$", Address="something" };
        var command = new AddSupportTicketCommand
        {
            Description = "Issue with policy renewal",
            TicketType = TicketType.Appointment,
            AppointmentDate = new DateTime(2025, 6, 1, 14, 0, 0),
            Status = TicketStatus.New,
            Client = client
        };

        var mappedTicket = new SupportTicket
        {
            Description = command.Description,
            TicketType = command.TicketType,
            AppointmentDate = command.AppointmentDate,
            Status = command.Status,
            Client = client
        };

        _mockMapper
            .Setup(m => m.Map<SupportTicket>(command))
            .Returns(mappedTicket);

        var mockRepo = new Mock<IRepository<SupportTicket, int>>();
        _mockUnitOfWork.SetupGet(u => u.SupportTickets).Returns(mockRepo.Object);

        // Act
        var response = await _ticketsHandler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Data.Should().BeSameAs(mappedTicket);

        mockRepo.Verify(r => r.Add(mappedTicket), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldAddClient_WhenCommandIsValid()
    {
        var command = new AddClientCommand
        {
            FirstName = "Alice",
            LastName = "Smith",
            Email = "alice@example.com",
            Phone = "+123456789",
            Address = "123 Main St",
            Password = "P@ssw0rd!",
            DateOfBirth = new DateTime(1990, 5, 20),
            Gender = Gender.Female,
            Status = AvailabilityStatus.Active,
            NumberOfDeals = 3
        };

        var mappedClient = new Client
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Phone = command.Phone,
            Address = command.Address,
            Password = "hashed", 
            DateOfBirth = command.DateOfBirth,
            Gender = command.Gender,
            Status = command.Status,
            NumberOfDeals = command.NumberOfDeals
        };

        _mockMapper
            .Setup(m => m.Map<Client>(command))
            .Returns(mappedClient);

        var mockRepo = new Mock<IRepository<Client, int>>();
        _mockUnitOfWork.SetupGet(u => u.Clients).Returns(mockRepo.Object);

        // Act
        var response = await _clientsHandler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Data.Should().BeSameAs(mappedClient);

        mockRepo.Verify(r => r.Add(mappedClient), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}