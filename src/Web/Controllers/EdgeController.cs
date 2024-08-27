using Application.Interfaces.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.Edge;
using Web.Helper;
using Web.Identity;
using Web.Mappers;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class EdgeController : ControllerBase
{
     private readonly IEdgeService _edgeService;

    public EdgeController(IEdgeService edgeService)
    {
        _edgeService = edgeService;
    }

    [HttpPost]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ImportEdges([FromForm] ImportEdgesRequest request)
    {
        if (request.File.Length == 0)
            return BadRequest("No file uploaded.");

        var filePath = Path.GetTempFileName();

        await using (var stream = System.IO.File.Create(filePath))
        {
            await request.File.CopyToAsync(stream);
        }

        var result = await _edgeService.AddEdgesFromCsvAsync(filePath, 
            request.SourceColumn, 
            request.DestinationColumn, 
            request.TypeLabelColumn, 
            request.IdColumn);

        if (!result.Succeed)
        {
            return BadRequest(result.Message);
        }

        return Ok();
    }

    [HttpGet]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin, AppRoles.DataAnalyst)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetAllEdges()
    {
        var allEdges = await _edgeService.GetAllEdgesAsync();
        if (!allEdges.Succeed)
        {
            return BadRequest(Errors.New(nameof(GetAllEdges), allEdges.Message));
        }

        var response = allEdges.Value!;
        return Ok(response.ToGotAllEdgesDto());
    }
    
    [HttpGet("{nodeId}")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin, AppRoles.DataAnalyst)]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetEdgesByNodeId(long nodeId)
    {
        var edges = await _edgeService.GetEdgesByNodeIdAsync(nodeId);
        var edgeDtos = edges.ToGotAllEdgesDto();
        return Ok(edgeDtos);
    }
}