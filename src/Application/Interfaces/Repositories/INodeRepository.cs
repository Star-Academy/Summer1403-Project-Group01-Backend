using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface INodeRepository
{
    Task<List<Node>> GetAllAsync();
    Task<Node?> GetByIdAsync(long nodeId);
    Task AddRangeAsync(List<Node> nodes);
}