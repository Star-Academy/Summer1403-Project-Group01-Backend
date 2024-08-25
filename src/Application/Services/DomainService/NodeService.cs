using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services.DomainService;

public class NodeService : INodeService
{
    private readonly INodeRepository _nodeRepository;

    public NodeService(INodeRepository nodeRepository)
    {
        _nodeRepository = nodeRepository;
    }

    public async Task<Node?> GetNodeByIdAsync(long nodeId)
    {
        return await _nodeRepository.GetByIdAsync(nodeId);
    }

    public async Task<Result<List<Node>>> GetAllNodesAsync()
    {
        try
        {
            var accounts = await _nodeRepository.GetAllAsync();
            return Result<List<Node>>.Ok(accounts);
        }
        catch (Exception ex)
        {
            return Result<List<Node>>.Fail($"An error occurred: {ex.Message}");
        }
    }
}