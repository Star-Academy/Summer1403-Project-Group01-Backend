using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NodeRepository : INodeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public NodeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Node>> GetAllAsync()
    {
        return await _dbContext.Nodes.ToListAsync();
    }

    public async Task AddRangeAsync(List<Node> nodes)
    {
        await _dbContext.Nodes.AddRangeAsync(nodes);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Node?> GetByIdAsync(long nodeId)
    {
        return await _dbContext.Nodes.FindAsync(nodeId);
    }
}