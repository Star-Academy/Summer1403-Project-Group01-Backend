using Application.DTOs;
using Application.DTOs.Edge;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.Services.SharedService;
using Domain.Entities;

namespace Application.Services.DomainService;

public class EdgeService : IEdgeService
{
    private readonly IEdgeRepository _edgeRepository;
    private readonly IEdgeAttributeRepository _edgeAttributeRepository;

    public EdgeService(IEdgeRepository edgeRepository, IEdgeAttributeRepository edgeAttributeRepository)
    {
        _edgeRepository = edgeRepository;
        _edgeAttributeRepository = edgeAttributeRepository;
    }

    public async Task<Result> AddEdgesFromCsvAsync(string filePath)
    {
        var edgeCsvModels = CsvReaderService.ReadFromCsv<EdgeCsvModel>(filePath);
        
        var availableAttributes = await _edgeAttributeRepository.GetAllAsync();
        
        var edges = edgeCsvModels
            .Select(csvModel => csvModel.ToEdge(availableAttributes))
            .ToList();
        
        try
        {
            await _edgeRepository.CreateBulkAsync(edges);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"An error occurred: {ex.Message}");
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