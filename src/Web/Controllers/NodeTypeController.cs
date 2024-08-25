using Application.DTOs.NodeType;
using Application.Interfaces.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.NodeType;
using Web.Helper;
using Web.Identity;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NodeTypeController : ControllerBase
{
    private readonly INodeTypeService _nodeTypeService;

    public NodeTypeController(INodeTypeService nodeTypeService)
    {
        _nodeTypeService = nodeTypeService;
    }

    [HttpPost]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] NodeTypeDto nodeTypeDto)
    {
        var result = await _nodeTypeService.CreateNodeTypeAsync(new CreateNodeTypeRequest
        {
            Label = nodeTypeDto.Label
        });

        if (!result.Succeed)
        {
            return BadRequest(Errors.New(nameof(Create), result.Message));
        }

        return Ok();
    }
    
    [HttpPost]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _nodeTypeService.GetAllNodeTypesAsync();
        
        if (!result.Succeed)
        {
            return BadRequest(Errors.New(nameof(Create), result.Message));
        }

        return Ok(result.Value);
    }
}