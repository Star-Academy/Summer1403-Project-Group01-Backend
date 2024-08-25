using Application.DTOs;
using Application.DTOs.Node;

namespace Application.Interfaces.Services;

public interface INodeLoader
{
    Task<Result> LoadFromFile(LoadNodesFromFileRequest data);
}