using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost]
    public async Task<IActionResult> ImportAccounts([FromForm] IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("No file uploaded.");

        var filePath = Path.GetTempFileName();

        await using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        await _accountService.AddAccountsFromCsvAsync(filePath);

        return Ok("Accounts imported successfully.");
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(int id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        if (account == null)
        {
            return NotFound();
        }

        return Ok(account);
    }
}