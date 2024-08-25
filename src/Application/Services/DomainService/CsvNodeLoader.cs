using System.Globalization;
using Application.DTOs;
using Application.DTOs.Node;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using CsvHelper;
using Domain.Entities;

namespace Application.Services.DomainService;

public class CsvNodeLoader : INodeLoader
{
    private readonly INodeRepository _nodeRepository;
    private readonly INodeAttributeRepository _nodeAttributeRepository;
    private readonly INodeTypeRepository _nodeTypeRepository;
    
    public CsvNodeLoader(INodeRepository nodeRepository, INodeAttributeRepository nodeAttributeRepository, INodeTypeRepository nodeTypeRepository)
    {
        _nodeRepository = nodeRepository;
        _nodeAttributeRepository = nodeAttributeRepository;
        _nodeTypeRepository = nodeTypeRepository;
    }

    public async Task<Result> LoadFromFile(LoadNodesFromFileRequest data)
    {
        using var reader = new StreamReader(data.FilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        await csv.ReadAsync();
        csv.ReadHeader();
        
        var headers = csv.HeaderRecord;
        
        var validAttributes = await _nodeAttributeRepository.GetAllAsync();
        
        validAttributes.RemoveAll(attribute => attribute.NodeType?.Label != data.NodeType);

        var nodeType = await _nodeTypeRepository.GetByLabelAsync(data.NodeType);
        
        if (nodeType is null)
        {
            return Result.Fail("Invalid node type");
        }
        
        if (!AreHeadersValid(headers, data.Aliases, validAttributes))
        {
            return Result.Fail("Invalid headers");
        }

        var nodes = new List<Node>();
        while (await csv.ReadAsync())
        {
            nodes.Add(GetNextNode(csv, headers!, data.Aliases, nodeType, validAttributes));
        }

        await _nodeRepository.AddRangeAsync(nodes);
        
        return Result.Ok();
    }

    private Node GetNextNode(CsvReader csv, string[] headers, Dictionary<string, string> aliases,
        NodeType nodeType, List<NodeAttribute> validAttributes)
    {
        var node = new Node();
        foreach (var header in headers)
        {
            var fieldValue = csv.GetField(header);
            var fixedHeader = FixHeaderName(header, aliases);
            var attribute = validAttributes.First(a => a.Label == fixedHeader);
            
            if (fieldValue == null)
            {
                continue;
            }
            
            node.AttributeValues.Add(new NodeAttributeValue
            {
                NodeId = node.Id,
                NodeAttributeId = attribute.Id,
                Value = fieldValue!
            });
        }

        return node;
    }

    private static bool AreHeadersValid(string[]? headers, Dictionary<string, string> aliases,
        List<NodeAttribute> validAttributes)
    {
        if (headers == null)
        {
            return false;
        }

        return headers.All(header => 
            validAttributes.Any(a => a.Label == FixHeaderName(header, aliases)));
    }

    private static string FixHeaderName(string header, Dictionary<string, string> aliases)
    {
        return aliases.GetValueOrDefault(header, header);
    }
    
}