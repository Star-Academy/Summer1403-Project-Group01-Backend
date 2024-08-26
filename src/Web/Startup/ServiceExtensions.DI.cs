﻿using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services.DomainService;
using Infrastructure.Repositories;
using Web.Services;

namespace Web.Startup;

public static partial class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRoleManagerRepository, RoleManagerRepository>();
        services.AddScoped<IUserManagerRepository, UserManagerRepository>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEdgeRepository, EdgeRepository>();
        services.AddScoped<IEdgeService, EdgeService>();
        services.AddScoped<IEdgeAttributeRepository, EdgeAttributeRepository>();
    }
}