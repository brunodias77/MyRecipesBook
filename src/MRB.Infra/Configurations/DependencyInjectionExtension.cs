using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRB.Domain.Repositories;
using MRB.Domain.Security;
using MRB.Infra.Data;
using MRB.Infra.Data.Repositories;

namespace MRB.Infra.Configurations;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        // if (configuration.IsUnitTestEnvironment())
        //     return;
        AddPasswordEncrypter(services, configuration);
        AddDbContext(services, configuration);
        AddLoggedUser(services);
        AddToken(services, configuration);
        AddRepositories(services);

        AddFluenteMigrator(services, configuration);
    }

    private static void AddDbContext(IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var version = new Version(8, 4, 0);
        var serverVersion = new MySqlServerVersion(version);

        services.AddDbContext<ApplicationDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }

    private static void AddToken(IServiceCollection services, IConfigurationManager configuration)
    {
        // var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        // var signingKey = configuration.GetValue<string>("Settings:Jwt:SigninKey");
        //
        // services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        // services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IRecipeRepository, RecipeRepository>();
        // services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddFluenteMigrator(IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options.AddMySql8().WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MRB.Infra")).For.All();
        });
    }

    private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter, Security.BCrypt>();

        // var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        // services.AddScoped<IPasswordEncripter>(options => new Sha512Encript(additionalKey!));
    }

    private static void AddLoggedUser(IServiceCollection services)
    {
        // services.AddScoped<ILoggedUser, LoggedUser>();
    }
}