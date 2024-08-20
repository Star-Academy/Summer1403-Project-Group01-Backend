using Application.DTOs.Transaction;
using Domain.Entities;

namespace Application.Mappers;
public static class TransactionMapper
{
    public static GetAllTransactionsResponse ToGetAllTransactionsResponse(this List<Transaction> transactions)
    {
        return new GetAllTransactionsResponse
        {
            Transactions = transactions.Select(transaction => new TransactionCsvModel
            {
                TransactionID = transaction.TransactionId,
                SourceAcount = transaction.SourceAccountId,
                DestiantionAccount = transaction.DestinationAccountId,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Type = transaction.Type
            }).ToList()
        };
    }
    
    public static Transaction ToTransaction(this TransactionCsvModel csvModel)
    {
        return new Transaction
        {
            TransactionId = csvModel.TransactionID,
            SourceAccountId = csvModel.SourceAcount,
            DestinationAccountId = csvModel.DestiantionAccount,
            Amount = csvModel.Amount,
            Date = csvModel.Date,
            Type = csvModel.Type
        };
    }
}