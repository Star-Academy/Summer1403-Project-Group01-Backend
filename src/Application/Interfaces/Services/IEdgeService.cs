using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IEdgeService
{
    Task<Result> AddEdgesFromCsvAsync(string filePath, string sourceColumn, string destinationColumn,
        string typeLabelColumn, string idColumn);
    Task<Result<List<Edge>>> GetAllEdgesAsync();
    Task<List<Edge>> GetEdgesByNodeIdAsync(long nodeId);
}