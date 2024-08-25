using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NodeAttributeRepository : INodeAttributeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public NodeAttributeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<NodeAttribute>> GetAllAsync()
    {
        return await _dbContext.NodeAttributes.ToListAsync();
    }

    public async Task<List<NodeAttribute>> GetAllByNodeTypeAsync(string nodeType)
    {
        return await _dbContext.NodeAttributes
            .Where(attribute => attribute.NodeType != null && attribute.NodeType.Label == nodeType)
            .ToListAsync();
    }

    public async Task AddAsync(NodeAttribute nodeAttribute)
    {
        await _dbContext.NodeAttributes.AddAsync(nodeAttribute);
        await _dbContext.SaveChangesAsync();
    }
}