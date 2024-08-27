using Application.DTOs.Node;
using Application.Interfaces.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.Node;
using Web.Helper;
using Web.Identity;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NodeController : ControllerBase
{
    private readonly INodeLoader _nodeLoader;
    private readonly ILogger<NodeController> _logger;

    public NodeController(INodeLoader nodeLoader, ILogger<NodeController> logger)
    {
        _nodeLoader = nodeLoader;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Import([FromForm] ImportNodesDto importNodesDto)
    {
        if (importNodesDto.File.Length == 0)
            return BadRequest(Errors.New(nameof(Import), "No file found."));

        var filePath = Path.GetTempFileName();
        
        _logger.LogInformation("Received NodeType: {NodeType}", importNodesDto.NodeType);
        _logger.LogInformation("Received Aliases: {Aliases}", string.Join(", ", importNodesDto.Aliases.Select(kv => $"{kv.Key}: {kv.Value}")));


        await using (var stream = System.IO.File.Create(filePath))
        {
            await importNodesDto.File.CopyToAsync(stream);
        }

        var result = await _nodeLoader.LoadFromFile(new LoadNodesFromFileRequest
        {
            FilePath = filePath,
            Aliases = importNodesDto.Aliases,
            NodeType = importNodesDto.NodeType
        });
        
        if (!result.Succeed)
        {
            return BadRequest(Errors.New(nameof(Import), result.Message));
        }
        
        return Ok();
    }
}