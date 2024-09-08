using Application.Interfaces.Repositories;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.AccessControl;

namespace Web.Controllers;

[ApiController]
[Route("file-ids")]
public class FileIdController : ControllerBase
{
    private readonly IFileIdRepository _fileIdRepository;

    public FileIdController(IFileIdRepository fileIdRepository)
    {
        _fileIdRepository = fileIdRepository;
    }
    
    [HttpGet]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    
    public async Task<IActionResult> GetAllFileIds()
    {
        var fileIds = await _fileIdRepository.GetAllIdsAsync();
        return Ok(fileIds);
    }
}