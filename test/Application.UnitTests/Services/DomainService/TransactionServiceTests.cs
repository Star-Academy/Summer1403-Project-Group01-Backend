﻿using Application.DTOs.Transaction;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services.DomainService;
using Domain.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace test.Application.UnitTests.Services.DomainService;

public class TransactionServiceTests
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IFileReaderService _fileReaderService;
    private readonly TransactionService _transactionService;
    private readonly IAccountRepository _accountRepository;
    private readonly IFileIdRepository _fileIdRepository;
    public TransactionServiceTests()
    {
        _fileIdRepository = Substitute.For<IFileIdRepository>();
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _fileReaderService = Substitute.For<IFileReaderService>();
        _accountRepository = Substitute.For<IAccountRepository>();
        _transactionService = new TransactionService(_transactionRepository, _fileReaderService, _accountRepository, _fileIdRepository);
    }

    [Fact]
    public async Task AddTransactionsFromCsvAsync_ShouldReturnOk_WhenTransactionsAreAddedSuccessfully()
    {
        // Arrange
        var filePath = "test.csv";
        var transactionCsvModels = new List<TransactionCsvModel>
        {
            new() { TransactionId = 1, SourceAccount = 101, DestinationAccount = 102, Amount = 100, Date = "05/24/2018", Time = "6:42:05", Type = "کارت به کارت", TrackingId = 12102},
            new() { TransactionId = 2, SourceAccount = 101, DestinationAccount = 103, Amount = 200, Date = "04/04/2017", Time = "4:12:08", Type = "ساتنا", TrackingId = 12103}
        };
        
        var expectedFirstDateTime = new DateTime(2018, 5, 24, 6, 42, 5);
        var expectedSecondDateTime = new DateTime(2017, 4, 4, 4, 12, 8);
        
        var existingTransactionIds = new List<long> { 3 };

        _fileReaderService.ReadFromFile<TransactionCsvModel>(filePath).Returns(transactionCsvModels);
        _transactionRepository.GetAllIdsAsync().Returns(existingTransactionIds);
        _transactionRepository.CreateBulkAsync(Arg.Any<List<Transaction>>()).Returns(Task.CompletedTask);
        _accountRepository.GetByIdAsync(101).Returns(new Account {FileId = 1});
        _accountRepository.GetByIdAsync(102).Returns(new Account {FileId = 1});
        _accountRepository.GetByIdAsync(103).Returns(new Account {FileId = 1});
        _fileIdRepository.IdExistsAsync(1).Returns(true);

        // Act
        var result = await _transactionService.AddTransactionsFromCsvAsync(filePath, 1);

        // Assert
        Assert.True(result.Succeed);
        
        // Verifying if the dates were converted correctly
        await _transactionRepository.Received(1).CreateBulkAsync(Arg.Is<List<Transaction>>(x =>
                x.Count == transactionCsvModels.Count &&
                x[0].Date == expectedFirstDateTime &&
                x[1].Date == expectedSecondDateTime
        ));
    }
    
    [Fact]
    public async Task AddTransactionsFromCsvAsync_ShouldOnlyAddNewTransactions_WhenSomeTransactionsAlreadyExist()
    {
        // Arrange
        var filePath = "test.csv";
        var transactionCsvModels = new List<TransactionCsvModel>
        {
            new() { TransactionId = 1, SourceAccount = 101, DestinationAccount = 102, Amount = 100, Date = "05/24/2018", Time = "10:00:00", Type = "کارت به کارت", TrackingId = 12101 },
            new() { TransactionId = 2, SourceAccount = 101, DestinationAccount = 103, Amount = 200, Date = "05/24/2018", Time = "11:00:00", Type = "ساتنا", TrackingId = 12102 },
            new() { TransactionId = 3, SourceAccount = 104, DestinationAccount = 105, Amount = 300, Date = "05/24/2018", Time = "12:00:00", Type = "ساتنا", TrackingId = 12103 }
        };
        
        var existingTransactionIds = new List<long> { 3 };
        
        var expectedFirstDateTime = new DateTime(2018, 5, 24, 10, 0, 0);
        var expectedSecondDateTime = new DateTime(2018, 5, 24, 11, 0, 0);

        _fileReaderService.ReadFromFile<TransactionCsvModel>(filePath).Returns(transactionCsvModels);
        _transactionRepository.GetAllIdsAsync().Returns(existingTransactionIds);
        _transactionRepository.CreateBulkAsync(Arg.Any<List<Transaction>>()).Returns(Task.CompletedTask);
        _accountRepository.GetByIdAsync(101).Returns(new Account {FileId = 1});
        _accountRepository.GetByIdAsync(102).Returns(new Account {FileId = 1});
        _accountRepository.GetByIdAsync(103).Returns(new Account {FileId = 1});
        _accountRepository.GetByIdAsync(104).Returns(new Account {FileId = 1});
        _accountRepository.GetByIdAsync(105).Returns(new Account {FileId = 1});
        _fileIdRepository.IdExistsAsync(1).Returns(true);

        // Act
        var result = await _transactionService.AddTransactionsFromCsvAsync(filePath, 1);

        // Assert
        Assert.True(result.Succeed);

        // Only the new transactions (TransactionID = 1 and TransactionID = 2) should be added.
        await _transactionRepository.Received(1).CreateBulkAsync(Arg.Is<List<Transaction>>(x =>
            x.Count == 2 &&
            x.Any(t => t.TransactionId == 1 && t.Date == expectedFirstDateTime) &&
            x.Any(t => t.TransactionId == 2 && t.Date == expectedSecondDateTime) &&
            x.All(t => t.TransactionId != 3)
        ));
    }

    [Fact]
    public async Task AddTransactionsFromCsvAsync_ShouldReturnFail_WhenExceptionIsThrown()
    {
        // Arrange
        var filePath = "test.csv";
        var exceptionMessage = "An error occurred while reading the file.";
            
        _fileReaderService
            .ReadFromFile<TransactionCsvModel>(filePath)
            .Throws(new Exception(exceptionMessage));
        _fileIdRepository.IdExistsAsync(1).Returns(false);
        
        // Act
        var result = await _transactionService.AddTransactionsFromCsvAsync(filePath, 1);

        // Assert
        Assert.False(result.Succeed);
        Assert.Equal($"An error occurred: {exceptionMessage}", result.Message);
    }
    
    [Fact]
    public async Task GetAllTransactionsAsync_ShouldReturnAllTransactions()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new() { TransactionId = 1, SourceAccountId = 101, DestinationAccountId = 102, Amount = 100, Date = DateTime.UtcNow, Type = "کارت به کارت" },
            new() { TransactionId = 2, SourceAccountId = 101, DestinationAccountId = 103, Amount = 200, Date = DateTime.UtcNow, Type = "ساتنا" }
        };

        _transactionRepository.GetAllTransactions().Returns(transactions);
        

        // Act
        var result = await _transactionService.GetAllTransactionsAsync();

        // Assert
        Assert.True(result.Succeed);
        Assert.Equal(transactions, result.Value);
    }
    
    [Fact]
    public async Task GetAllTransactionsAsync_ShouldReturnFailResult_WhenExceptionIsThrown()
    {
        // Arrange
        var exceptionMessage = "Database connection failed.";
        _transactionRepository.GetAllTransactions().Throws(new Exception(exceptionMessage));

        // Act
        var result = await _transactionService.GetAllTransactionsAsync();

        // Assert
        Assert.False(result.Succeed);
        Assert.Null(result.Value);
        Assert.Equal($"An error occurred: {exceptionMessage}", result.Message);
    }

    [Fact]
    public async Task GetAllTransactionsAsync_ShouldReturnEmptyList_WhenNoTransactionsAreFound()
    {
        // Arrange
        var transactions = new List<Transaction>();
        _transactionRepository.GetAllTransactions().Returns(Task.FromResult(transactions));

        // Act
        var result = await _transactionService.GetAllTransactionsAsync();

        // Assert
        Assert.True(result.Succeed);
        Assert.Empty(result.Value!);
    }
    
    [Fact]
    public async Task GetTransactionsByAccountIdAsync_ShouldReturnOkResult_WhenTransactionsAreRetrievedAndGroupedSuccessfully()
    {
        // Arrange
        long accountId = 101;
        var transactionsSourceAccount = new List<Transaction>
        {
            new Transaction { TransactionId = 1, SourceAccountId = 101, DestinationAccountId = 102, Amount = 100, Date = new DateTime(2023, 7, 5), Type = "کارت به کارت" },
            new Transaction { TransactionId = 2, SourceAccountId = 101, DestinationAccountId = 103, Amount = 200, Date = new DateTime(2023, 7, 6), Type = "ساتنا" }
        };
        
        var transactionsDestinationAccount = new List<Transaction>
        {
            new Transaction { TransactionId = 3, SourceAccountId = 104, DestinationAccountId = 101, Amount = 300, Date = new DateTime(2023, 7, 7), Type = "ساتنا" },
            new Transaction { TransactionId = 4, SourceAccountId = 105, DestinationAccountId = 101, Amount = 400, Date = new DateTime(2023, 7, 8), Type = "کارت به کارت" }
        };

        _transactionRepository.GetBySourceAccountId(accountId).Returns(transactionsSourceAccount);
        _transactionRepository.GetByDestinationAccountId(accountId).Returns(transactionsDestinationAccount);

        // Act
        var result = await _transactionService.GetTransactionsByAccountIdAsync(accountId);

        // Assert
        Assert.True(result.Succeed); 
        Assert.Equal(4, result.Value!.Count);
    }
    
    [Fact]
    public async Task GetTransactionsByAccountIdAsync_ShouldReturnOkResult_WhenThereAreMultipleTransactionsBetweenTwoAccounts()
    {
        // Arrange
        long accountId = 101;
        var transactionsSourceAccount = new List<Transaction>
        {
            new Transaction { TransactionId = 1, SourceAccountId = 101, DestinationAccountId = 102, Amount = 100, Date = new DateTime(2023, 7, 5), Type = "کارت به کارت" },
            new Transaction { TransactionId = 2, SourceAccountId = 101, DestinationAccountId = 102, Amount = 200, Date = new DateTime(2023, 7, 6), Type = "ساتنا" }
        };
        
        var transactionsDestinationAccount = new List<Transaction>
        {
            new Transaction { TransactionId = 3, SourceAccountId = 104, DestinationAccountId = 101, Amount = 300, Date = new DateTime(2023, 7, 7), Type = "ساتنا" },
            new Transaction { TransactionId = 4, SourceAccountId = 102, DestinationAccountId = 101, Amount = 400, Date = new DateTime(2023, 7, 8), Type = "کارت به کارت" }
        };

        _transactionRepository.GetBySourceAccountId(accountId).Returns(transactionsSourceAccount);
        _transactionRepository.GetByDestinationAccountId(accountId).Returns(transactionsDestinationAccount);

        // Act
        var result = await _transactionService.GetTransactionsByAccountIdAsync(accountId);

        // Assert
        Assert.True(result.Succeed);
        Assert.Equal(4, result.Value!.SelectMany(x => x.TransactionWithSources).Count());
        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task GetTransactionsByAccountIdAsync_ShouldReturnEmptyList_WhenNoTransactionsAreFound()
    {
        // Arrange
        long accountId = 101;
        _transactionRepository.GetBySourceAccountId(accountId).Returns(new List<Transaction>());
        _transactionRepository.GetByDestinationAccountId(accountId).Returns(new List<Transaction>());

        // Act
        var result = await _transactionService.GetTransactionsByAccountIdAsync(accountId);

        // Assert
        Assert.True(result.Succeed);
        Assert.Empty(result.Value!);
    }

    [Fact]
    public async Task GetTransactionsByAccountIdAsync_ShouldReturnFailResult_WhenExceptionIsThrown()
    {
        // Arrange
        long accountId = 101;
        var exceptionMessage = "Database error";
        _transactionRepository.GetBySourceAccountId(accountId).Throws(new Exception(exceptionMessage));

        // Act
        var result = await _transactionService.GetTransactionsByAccountIdAsync(accountId);

        // Assert
        Assert.False(result.Succeed);
        Assert.Null(result.Value);
        Assert.Equal($"An error occurred: {exceptionMessage}", result.Message);
    }
    
    
}