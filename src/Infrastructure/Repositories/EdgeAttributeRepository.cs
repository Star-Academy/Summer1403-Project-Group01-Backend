using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EdgeAttributeRepository : IEdgeAttributeRepository
{
    private readonly ApplicationDbContext _dbContext;
    public EdgeAttributeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<EdgeAttribute>> GetAllAsync()
    {
        return await _dbContext.EdgeAttributes.ToListAsync();
    }
}