using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EdgeTypeRepository : IEdgeTypeRepository
{
    private readonly ApplicationDbContext _dbContext;
    public EdgeTypeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EdgeType> GetByLabelAsync(string label)
    {
        return await _dbContext.EdgeTypes
            .FirstOrDefaultAsync(et => et.Label == label);
    }
}