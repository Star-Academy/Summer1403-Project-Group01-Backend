using Application.DTOs.Account;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.Services.DomainService;
using Application.Services.SharedService;
using Domain.Entities;
using NSubstitute;

namespace test.Application.UnitTests.Services.DomainService;

public class AccountServiceTests
{
    private readonly IAccountRepository _accountRepository;
    private readonly AccountService _accountService;
    private readonly IFileReaderService _fileReaderService;

    public AccountServiceTests()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _fileReaderService = Substitute.For<IFileReaderService>();
        _accountService = new AccountService(_accountRepository, _fileReaderService);
    }
    
    [Fact]
    public async Task AddAccountsFromCsvAsync_WhenCsvIsValid_ReturnsOk()
    {
        // Arrange
        var filePath = "dummy.csv";
        var accountCsvModels = new List<AccountCsvModel>
        {
            new AccountCsvModel
            {
                AccountId = 1,
                CardId = 100,
                Iban = "IBAN1",
                AccountType = "Savings",
                BranchTelephone = "1234567890",
                BranchAddress = "Main Street",
                BranchName = "Main Branch",
                OwnerName = "Mobin",
                OwnerLastName = "Barfi",
                OwnerId = 101
            },
            new AccountCsvModel
            {
                AccountId = 2,
                CardId = 200,
                Iban = "IBAN2",
                AccountType = "Checking",
                BranchTelephone = "0987654321",
                BranchAddress = "Some Street",
                BranchName = "Some Branch",
                OwnerName = "Mohammad",
                OwnerLastName = "Mohammadi",
                OwnerId = 102
            }
        };
        var existingAccountIds = new List<long> { 1 };

        _fileReaderService.ReadFromFile<AccountCsvModel>(filePath).Returns(accountCsvModels);
        _accountRepository.GetAllIdsAsync().Returns(existingAccountIds);

        // Act
        var result = await _accountService.AddAccountsFromCsvAsync(filePath);

        // Assert
        Assert.True(result.Succeed);
        await _accountRepository
            .Received(1)
            .CreateBulkAsync(Arg.Is<List<Account>>(a =>
                a.Count == 1 
                && a.First().AccountId == 2
                && a.First().OwnerName == "Mohammad"));
    }

    [Fact]
    public async Task AddAccountsFromCsvAsync_WhenExceptionIsThrown_ReturnsFail()
    {
        // Arrange
        var filePath = "dummy.csv";
        _fileReaderService
            .When(x => x.ReadFromFile<AccountCsvModel>(filePath))
            .Do(x => { throw new Exception("CSV read error"); });

        // Act
        var result = await _accountService.AddAccountsFromCsvAsync(filePath);

        // Assert
        Assert.False(result.Succeed);
        Assert.Contains("An unexpected error occurred: CSV read error", result.Message);
    }

    [Fact]
    public async Task GetAccountByIdAsync_WhenAccountExists_ReturnsAccount()
    {
        // Arrange
        var accountId = 1;
        var account = new Account { AccountId = accountId, OwnerName = "John Doe" };

        _accountRepository.GetByIdAsync(accountId).Returns(account);

        // Act
        var result = await _accountService.GetAccountByIdAsync(accountId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(accountId, result.Value!.AccountId);
        Assert.Equal("John Doe", result.Value!.OwnerName);
    }

    [Fact]
    public async Task GetAccountByIdAsync_WhenAccountDoesNotExist_ReturnsNull()
    {
        // Arrange
        var accountId = 1;
        _accountRepository.GetByIdAsync(accountId).Returns((Account?)null);

        // Act
        var result = await _accountService.GetAccountByIdAsync(accountId);

        // Assert
        Assert.False(result.Succeed);
        Assert.Null(result.Value);
        Assert.Equal("Account not found", result.Message);
    }

    [Fact]
    public async Task GetAllAccountsAsync_WhenAccountsExist_ReturnsListOfAccounts()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new Account { AccountId = 1, OwnerName = "Mobin Barfi" },
            new Account { AccountId = 2, OwnerName = "Mohammad Mohammadi" }
        };

        _accountRepository.GetAllAccounts().Returns(accounts);

        // Act
        var result = await _accountService.GetAllAccountsAsync();

        // Assert
        Assert.True(result.Succeed);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal("Mobin Barfi", result.Value[0].OwnerName);
        Assert.Equal("Mohammad Mohammadi", result.Value[1].OwnerName);
    }

    [Fact]
    public async Task GetAllAccountsAsync_WhenExceptionIsThrown_ReturnsFail()
    {
        // Arrange
        _accountRepository
            .When(x => x.GetAllAccounts())
            .Do(x => throw new Exception("Database error"));

        // Act
        var result = await _accountService.GetAllAccountsAsync();

        // Assert
        Assert.False(result.Succeed);
        Assert.Equal("An unexpected error occurred: Database error", result.Message);
    }
}