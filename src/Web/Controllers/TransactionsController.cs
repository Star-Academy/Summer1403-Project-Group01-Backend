using Application.Interfaces.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.AccessControl;
using Web.Helper;
using Web.Mappers;

namespace Web.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("upload")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> UploadTransactions([FromForm] IFormFile file, [FromForm] long fileId)
    {
        if (file.Length == 0)
            return BadRequest("No file uploaded.");

        var filePath = Path.GetTempFileName();

        await using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        var result = await _transactionService.AddTransactionsFromCsvAsync(filePath, fileId);
        
        if (!result.Succeed)
        {
            var errorResponse = Errors.New(nameof(UploadTransactions), result.Message);
            return BadRequest(errorResponse);
        }
        
        return Ok(result.Message);
    }

    [HttpGet]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin, AppRoles.DataAnalyst)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetAllTransactions()
    {
        var allTransactions = await _transactionService.GetAllTransactionsAsync();
        if (!allTransactions.Succeed)
        {
            var errorResponse = Errors.New(nameof(GetAllTransactions), allTransactions.Message);
            return BadRequest(errorResponse);
        }

        var response = allTransactions.Value!;
        return Ok(response.ToGotAllTransactionsDto());
    }
    
    [HttpGet("by-account/{accountId}")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin, AppRoles.DataAnalyst)]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetTransactionsByAccountId(long accountId)
    {
        var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);

        if (!transactions.Succeed)
        {
            var errorResponse = Errors.New(nameof(GetAllTransactions), transactions.Message);
            return BadRequest(errorResponse);
        }

        var response = transactions.Value!;
        
        return Ok(response);
    }
    
    [HttpGet("by-file-id/{fileId}")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin, AppRoles.DataAnalyst)]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetTransactionsByFileId(long fileId)
    {
        var transactions = await _transactionService.GetTransactionsByFileIdAsync(fileId);

        if (!transactions.Succeed)
        {
            var errorResponse = Errors.New(nameof(GetAllTransactions), transactions.Message);
            return BadRequest(errorResponse);
        }

        var response = transactions.Value!.ToGotAllTransactionsDto();
        
        return Ok(response);
    }
    
    [HttpDelete("{fileId}")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin, AppRoles.DataAdmin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> DeleteTransactionsByFileId(long fileId)
    {
        var result = await _transactionService.DeleteTransactionsByFileIdAsync(fileId);

        if (!result.Succeed)
        {
            var errorResponse = Errors.New(nameof(DeleteTransactionsByFileId), result.Message);
            return BadRequest(errorResponse);
        }

        return Ok(result.Message);
    }
}