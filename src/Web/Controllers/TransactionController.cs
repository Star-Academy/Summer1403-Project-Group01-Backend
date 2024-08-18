using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Helper;
using Web.Mappers;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TransactionController : ControllerBase
{
    public readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost]
    public async Task<IActionResult> ImportTransactions([FromForm] IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("No file uploaded.");

        var filePath = Path.GetTempFileName();

        await using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        await _transactionService.AddTransactionsFromCsvAsync(filePath);

        return Ok("Transactions imported successfully.");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllTransactions()
    {
        var allTransactions = await _transactionService.GetAllTransactions();
        if (!allTransactions.Succeed)
        {
            return BadRequest(Errors.New(nameof(GetAllTransactions), allTransactions.Message));
        }

        var response = allTransactions.Value!;
        return Ok(response.ToGotAllTransactionsDto());
    }
}