using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface INodeTypeRepository
{
    Task<List<NodeType>> GetAllAsync();
    Task<NodeType?> GetByLabelAsync(string label);
    Task AddAsync(NodeType nodeType);
}