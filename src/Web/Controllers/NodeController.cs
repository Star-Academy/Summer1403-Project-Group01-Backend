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
    private INodeLoader _nodeLoader;

    public NodeController(INodeLoader nodeLoader)
    {
        _nodeLoader = nodeLoader;
    }

    [HttpPost]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Import([FromForm] IFormFile file, [FromBody] ImportNodesDto importNodesDto)
    {
        if (file.Length == 0)
            return BadRequest(Errors.New(nameof(Import), "No file found."));

        var filePath = Path.GetTempFileName();

        await using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
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