using Web.DTOs.Transaction;
using Domain.Entities;

namespace Application.Mappers
{
    public static class TransactionMapper
    {
        public static GetAllTransactionsResponse ToGetAllTransactionsResponse(this List<Transaction> transactions)
        {
            return new GetAllTransactionsResponse
            {
                Transactions = transactions.Select(transaction => new GetAllTransactionsResponse.TransactionDto
                {
                    TransactionId = transaction.TransactionId,
                    SourceAccountId = transaction.SourceAccountId,
                    DestinationAccountId = transaction.DestinationAccountId,
                    Amount = transaction.Amount,
                    Date = transaction.Date,
                    Type = transaction.Type
                }).ToList()
            };
        }
    }
}