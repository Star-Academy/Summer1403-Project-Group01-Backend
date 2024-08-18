using Domain.Entities;

namespace Application.Interfaces;

public interface IAccountRepository
{
    Task CreateBulkAsync(List<Account> accounts);
}