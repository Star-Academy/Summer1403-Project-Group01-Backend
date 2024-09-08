using Application.Interfaces.Services;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.AccessControl;
using Web.Helper;
using Web.Mappers;

namespace Web.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("upload")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UploadAccounts([FromForm] IFormFile file, [FromForm] long fileId)
    {
        if (file.Length == 0)
            return BadRequest("No file uploaded.");

        var filePath = Path.GetTempFileName();

        await using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }
        
        var result = await _accountService.AddAccountsFromCsvAsync(filePath, fileId);
        if (!result.Succeed)
        {
            var errorResponse = Errors.New(nameof(UploadAccounts), result.Message);
            return StatusCode((int)result.ErrCode, errorResponse);
        }
        
        return Ok("Accounts uploaded successfully!");
    }

    [HttpGet("{accountId}")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin, AppRoles.DataAnalyst)]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAccountById(long accountId)
    {
        var account = await _accountService.GetAccountByIdAsync(accountId);
        if (!account.Succeed)
        {
            var errorResponse = Errors.New(nameof(GetAccountById), account.Message);
            return StatusCode((int)account.ErrCode, errorResponse);
        }

        var response = account.Value!;

        return Ok(response.ToAccountDto());
    }

    [HttpGet]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin, AppRoles.DataAnalyst)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAllAccounts()
    {
        var allAccounts = await _accountService.GetAllAccountsAsync();
        if (!allAccounts.Succeed)
        {
            var errorResponse = Errors.New(nameof(GetAllAccounts), allAccounts.Message);
            return StatusCode((int)allAccounts.ErrCode, errorResponse);
        }

        var response = allAccounts.Value!;
        return Ok(response.ToGotAllAccountsDto());
    }
    
    [HttpGet("by-file-id/{fileId}")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin, AppRoles.DataAnalyst)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAccountsByFileId(long fileId)
    {
        var accounts = await _accountService.GetAccountsByFileIdAsync(fileId);
        if (!accounts.Succeed)
        {
            var errorResponse = Errors.New(nameof(GetAccountsByFileId), accounts.Message);
            return StatusCode((int)accounts.ErrCode, errorResponse);
        }

        var response = accounts.Value!;
        return Ok(response.ToGotAllAccountsDto());
    }
    
    [HttpDelete("{fileId}")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> DeleteAccountsByFileId(long fileId)
    {
        var result = await _accountService.DeleteAccountsByFileIdAsync(fileId);
        if (!result.Succeed)
        {
            var errorResponse = Errors.New(nameof(DeleteAccountsByFileId), result.Message);
            return StatusCode((int)result.ErrCode, errorResponse);
        }

        return Ok("Accounts deleted successfully!");
    }
}