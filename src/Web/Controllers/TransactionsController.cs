using Application.Interfaces.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Helper;
using Web.Identity;
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
    public async Task<IActionResult> UploadTransactions([FromForm] IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("No file uploaded.");

        var filePath = Path.GetTempFileName();

        await using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        var result = await _transactionService.AddTransactionsFromCsvAsync(filePath);
        
        if (!result.Succeed)
        {
            return BadRequest(result.Message);
        }
        
        return Ok();
    }

    [HttpGet()]
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
            return BadRequest(Errors.New(nameof(GetAllTransactions), allTransactions.Message));
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
            return BadRequest(Errors.New(nameof(GetAllTransactions), transactions.Message));
        }

        var response = transactions.Value!;
        
        return Ok(response);
    }
}