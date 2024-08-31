using Application.DTOs;
using Domain.Entities;
using Web.DTOs.Transaction;

namespace Web.Mappers;

public static class TransactionMapper
{
    public static TransactionDto ToTransactionDto(this Transaction transaction)
    {
        return new TransactionDto
        {
            TransactionId = transaction.TransactionId,
            SourceAccountId = transaction.SourceAccountId,
            DestinationAccountId = transaction.DestinationAccountId,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Type = transaction.Type 
        };
    }
    
    public static PaginatedList<TransactionDto> ToGotAllTransactionsDto(this PaginatedList<Transaction> transactions)
    {
        var dtoItems = transactions.Items.Select(transaction => new TransactionDto
        {
            TransactionId = transaction.TransactionId,
            SourceAccountId = transaction.SourceAccountId,
            DestinationAccountId = transaction.DestinationAccountId,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Type = transaction.Type
        }).ToList();

        return new PaginatedList<TransactionDto>(dtoItems, transactions.TotalCount, transactions.PageNumber, transactions.PageSize);
    }

}