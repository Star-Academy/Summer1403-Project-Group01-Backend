using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface INodeService
{
    Task<Node?> GetNodeByIdAsync(long nodeId);
    Task<Result<List<Node>>> GetAllNodesAsync();
}