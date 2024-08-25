using Application.DTOs;
using Application.DTOs.NodeAttribute;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface INodeAttributeService
{
    Task<Result<List<NodeAttribute>>> GetAllByNodeTypeAsync(GetNodeAttributesByNodeTypeRequest data);
    Task<Result> CreateNodeAttributeAsync(CreateNodeAttributeRequest data);
}