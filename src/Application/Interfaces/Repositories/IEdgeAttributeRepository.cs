using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IEdgeAttributeRepository
{
    Task<List<EdgeAttribute>> GetAllAsync();
    Task CreateBulkAsync(List<EdgeAttribute> newAttributes);
}