using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EdgeRepository : IEdgeRepository
{
    private readonly ApplicationDbContext _dbContext;
    public EdgeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateBulkAsync(List<Edge> edges)
    {
        await _dbContext.Edges.AddRangeAsync(edges);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Edge>> GetAllEdges()
    {
        return await _dbContext.Edges.ToListAsync();
    }

    public Task<List<Edge>> GetBySourceNodeId(long nodeId)
    {
        return _dbContext.Edges
            .Where(edge => edge.SourceValue == nodeId)
            .ToListAsync();
    }

    public Task<List<Edge>> GetByDestinationNodeId(long nodeId)
    {
        return _dbContext.Edges
            .Where(edge => edge.DestinationValue == nodeId)
            .ToListAsync();
    }
}