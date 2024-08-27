using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IEdgeTypeRepository
{
    Task<EdgeType> GetByLabelAsync(string label);
}