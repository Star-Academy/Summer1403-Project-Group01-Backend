using Application.DTOs.NodeAttribute;
using Application.DTOs.NodeType;
using Application.Interfaces.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.NodeAttribute;
using Web.DTOs.NodeType;
using Web.Helper;
using Web.Identity;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NodeAttributeController : ControllerBase
{
    private readonly INodeAttributeService _nodeAttributeService;

    public NodeAttributeController(INodeAttributeService nodeAttributeService)
    {
        _nodeAttributeService = nodeAttributeService;
    }

    [HttpPost]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] NodeAttributeDto nodeTypeDto)
    {
        var result = await _nodeAttributeService.CreateNodeAttributeAsync(new CreateNodeAttributeRequest
        {
            Label = nodeTypeDto.Label,
            NodeType = nodeTypeDto.NodeType
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
    public async Task<IActionResult> GetAllByNodeType([FromBody] NodeTypeDto nodeTypeDto)
    {
        var result = await _nodeAttributeService.GetAllByNodeTypeAsync(new GetNodeAttributesByNodeTypeRequest
        {
            NodeType = nodeTypeDto.Label
        });
        
        if (!result.Succeed)
        {
            return BadRequest(Errors.New(nameof(Create), result.Message));
        }

        return Ok(result.Value);
    }

}