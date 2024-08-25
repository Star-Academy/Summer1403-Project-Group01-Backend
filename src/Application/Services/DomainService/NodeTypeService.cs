using Application.DTOs;
using Application.DTOs.NodeType;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services.DomainService;

public class NodeTypeService : INodeTypeService
{
    private INodeTypeRepository _nodeTypeRepository;

    public NodeTypeService(INodeTypeRepository nodeTypeRepository)
    {
        _nodeTypeRepository = nodeTypeRepository;
    }
    
    public async Task<Result<List<NodeType>>> GetAllNodeTypesAsync()
    {
        try
        {
            var nodeTypes = await _nodeTypeRepository.GetAllAsync();
            return Result<List<NodeType>>.Ok(nodeTypes);
        }
        catch (Exception ex)
        {
            return Result<List<NodeType>>.Fail($"An error occurred: {ex.Message}");
        }
    }
    
    public async Task<Result> CreateNodeTypeAsync(CreateNodeTypeRequest data)
    {
        if (await _nodeTypeRepository.GetByLabelAsync(data.Label) is not null)
        {
            return Result.Fail("This node type already exists");
        }
        
        var nodeType = new NodeType
        {
            Label = data.Label
        };
        
        await _nodeTypeRepository.AddAsync(nodeType);

        return Result.Ok();
    }

}