using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IEdgeRepository
{
    Task CreateBulkAsync(List<Edge> edges);
    Task<List<Edge>> GetAllEdges();
    Task<List<Edge>> GetBySourceNodeId(long nodeId);
    Task<List<Edge>> GetByDestinationNodeId(long nodeId);
}