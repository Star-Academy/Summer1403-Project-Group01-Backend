using Application.DTOs;
using Application.DTOs.Edge;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.Services.SharedService;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Services.DomainService;

public class EdgeService : IEdgeService
{
    private readonly IEdgeRepository _edgeRepository;
    private readonly IEdgeAttributeRepository _edgeAttributeRepository;
    private readonly IEdgeTypeRepository _edgeTypeRepository;

    public EdgeService(IEdgeRepository edgeRepository, IEdgeAttributeRepository edgeAttributeRepository, IEdgeTypeRepository edgeTypeRepository)
    {
        _edgeRepository = edgeRepository;
        _edgeAttributeRepository = edgeAttributeRepository;
        _edgeTypeRepository = edgeTypeRepository;
    }

    public async Task<Result> AddEdgesFromCsvAsync(string filePath, string sourceColumn, string destinationColumn, string typeLabelColumn, string idColumn)
    {
        var edgeType = _edgeTypeRepository.GetByLabelAsync(typeLabelColumn);
        var edgeCsvModels = CsvReaderService.ReadFromCsv(filePath, sourceColumn, destinationColumn, edgeType.Id.ToString(), idColumn);

        var existingAttributes = await _edgeAttributeRepository.GetAllAsync();
        var existingLabels = new HashSet<string>(existingAttributes.Select(a => a.Label));

        var newAttributes = new List<EdgeAttribute>();

        foreach (var csvModel in edgeCsvModels)
        {
            var attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(csvModel.AttributesJson);

            foreach (var attr in attributes.Keys)
            {
                if (!existingLabels.Contains(attr))
                {
                    var newAttribute = new EdgeAttribute { Label = attr };
                    newAttributes.Add(newAttribute);
                    existingLabels.Add(attr); // Add the label to the set to avoid duplicate additions
                }
            }
        }

        if (newAttributes.Any())
        {
            try
            {
                await _edgeAttributeRepository.CreateBulkAsync(newAttributes);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to create new attributes: {ex.Message}");
            }
        }

        var edges = edgeCsvModels
            .Select(csvModel => csvModel.ToEdge(existingAttributes))
            .ToList();

        try
        {
            await _edgeRepository.CreateBulkAsync(edges);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"An error occurred while saving edges: {ex.Message}");
        }
    }

    public async Task<Result<List<Edge>>> GetAllEdgesAsync()
    {
        try
        {
            var edges = await _edgeRepository.GetAllEdges();
            return Result<List<Edge>>.Ok(edges);
        }
        catch (Exception ex)
        {
            return Result<List<Edge>>.Fail($"An error occurred: {ex.Message}");
        }
    }

    public async Task<List<Edge>> GetEdgesByNodeIdAsync(long nodeId)
    {
        var source = await _edgeRepository.GetBySourceNodeId(nodeId);
        var destination = await _edgeRepository.GetByDestinationNodeId(nodeId);
        return source.Concat(destination).ToList();
    }
}