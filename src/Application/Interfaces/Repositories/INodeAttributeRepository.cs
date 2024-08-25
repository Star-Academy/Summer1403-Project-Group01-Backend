using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface INodeAttributeRepository
{ 
    Task<List<NodeAttribute>> GetAllAsync();
    Task<List<NodeAttribute>> GetAllByNodeTypeAsync(string nodeType);
    Task AddAsync(NodeAttribute nodeAttribute);
}