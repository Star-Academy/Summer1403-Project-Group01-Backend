using Application.DTOs;
using Application.DTOs.NodeType;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface INodeTypeService
{
    Task<Result<List<NodeType>>> GetAllNodeTypesAsync();
    Task<Result> CreateNodeTypeAsync(CreateNodeTypeRequest data);
}