using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFileIdRepository
{
    Task<bool> IdExistsAsync(long fileId);
    Task AddAsync(FileId fileId);
    Task DeleteByIdAsync(long fileId);
    Task<List<FileId>> GetAllIdsAsync();
}