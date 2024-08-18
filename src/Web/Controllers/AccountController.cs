﻿using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Mappers;

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
    
    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetAccountById(long accountId)
    {
        var account = await _accountService.GetAccountByIdAsync(accountId);
        if (account == null)
        {
            return NotFound();
        }

        return Ok(account.ToAccountDto());
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetTransactionsByUserId(long accountId)
    {
        var result = await _accountService.GetTransactionsByUserId(accountId);
        if (!result.Succeed)
        {
            return NotFound("User do not exist");
        }

        var transactions = result.Value;
        
        return Ok(transactions!.Select(t => t.ToTransactionDto()));
    }
}