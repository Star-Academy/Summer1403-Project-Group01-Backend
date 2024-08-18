using Web.DTOs.Transaction;

namespace Web.Mappers
{
    public static class TransactionMapper
    {
        public static List<GetAllTransactionsResponse.TransactionDto> ToGotAllTransactionsDto(this GetAllTransactionsResponse response)
        {
            return response.Transactions.Select(transaction => new GetAllTransactionsResponse.TransactionDto
            {
                TransactionId = transaction.TransactionId,
                SourceAccountId = transaction.SourceAccountId,
                DestinationAccountId = transaction.DestinationAccountId,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Type = transaction.Type
            }).ToList();
        }
    }
}