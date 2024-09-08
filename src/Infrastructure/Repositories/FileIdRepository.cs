using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FileIdRepository : IFileIdRepository
{
    private ApplicationDbContext _dbContext;

    public FileIdRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IdExistsAsync(long fileId)
    {
        var id = await _dbContext.Files.FindAsync(fileId);
        return id != null;
    }

    public async Task AddAsync(FileId fileId)
    {
        await _dbContext.Files.AddAsync(fileId);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(long fileId)
    {
        var id = await _dbContext.Files.FindAsync(fileId);
        _dbContext.Files.Remove(id!);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<FileId>> GetAllIdsAsync()
    {
        return await _dbContext.Files.ToListAsync();
    }
}