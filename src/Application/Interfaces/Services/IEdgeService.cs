using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IEdgeService
{
    Task<Result> AddEdgesFromCsvAsync(string filePath);
    Task<Result<List<Edge>>> GetAllEdgesAsync();
    Task<List<Edge>> GetEdgesByNodeIdAsync(long nodeId);
}