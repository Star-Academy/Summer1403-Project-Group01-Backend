using Domain.Entities;

namespace Application.Interfaces;

public interface ITransactionRepository
{
    Task CreateBulkAsync(List<Transaction> transactions);
}