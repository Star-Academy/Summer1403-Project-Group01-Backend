using Application.DTOs;
using Application.DTOs.NodeAttribute;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services.DomainService;

public class NodeAttributeService : INodeAttributeService
{
    private readonly INodeAttributeRepository _nodeAttributeRepository;
    private readonly INodeTypeRepository _nodeTypeRepository;

    public NodeAttributeService(INodeAttributeRepository nodeAttributeRepository, INodeTypeRepository nodeTypeRepository)
    {
        _nodeAttributeRepository = nodeAttributeRepository;
        _nodeTypeRepository = nodeTypeRepository;
    }
    
    public async Task<Result<List<NodeAttribute>>> GetAllByNodeTypeAsync(GetNodeAttributesByNodeTypeRequest data)
    {
        try
        {
            var nodeAttributes = await _nodeAttributeRepository.GetAllByNodeTypeAsync(data.NodeType);
            return Result<List<NodeAttribute>>.Ok(nodeAttributes);
        }
        catch (Exception ex)
        {
            return Result<List<NodeAttribute>>.Fail($"An error occurred when getting the node attributes: {ex.Message}");
        }
    }
    
    public async Task<Result> CreateNodeAttributeAsync(CreateNodeAttributeRequest data)
    {
        var currentAttributes = await _nodeAttributeRepository.GetAllByNodeTypeAsync(data.NodeType);
        
        if (currentAttributes.Exists(attribute => attribute.Label == data.Label))
        {
            return Result.Fail("This node attribute already exists");
        }
        
        var nodeType = await _nodeTypeRepository.GetByLabelAsync(data.NodeType);

        if (nodeType is null)
        {
            return Result.Fail("This node type does not exist");
        }
        
        var nodeAttribute = new NodeAttribute()
        {
            Label = data.Label,
            NodeTypeId = nodeType.Id
        };
        
        await _nodeAttributeRepository.AddAsync(nodeAttribute);

        return Result.Ok();
    }
}