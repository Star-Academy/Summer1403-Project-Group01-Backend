using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NodeTypeRepository : INodeTypeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public NodeTypeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<NodeType>> GetAllAsync()
    {
        return await _dbContext.NodeTypes
            .Include(type => type.Attributes)
            .ToListAsync();
    }

    public async Task<NodeType?> GetByLabelAsync(string label)
    {
        return await _dbContext.NodeTypes.FirstOrDefaultAsync(x => x.Label == label);
    }

    public async Task AddAsync(NodeType nodeType)
    {
        await _dbContext.NodeTypes.AddAsync(nodeType);
        await _dbContext.SaveChangesAsync();
    }
}