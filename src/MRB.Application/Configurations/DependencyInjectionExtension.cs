using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRB.Application.Services;
using MRB.Application.UseCases.Recipes.Filter;
using MRB.Application.UseCases.Recipes.Register;
using MRB.Application.UseCases.Users.Login;
using MRB.Application.UseCases.Users.Profile;
using MRB.Application.UseCases.Users.Register;
using MRB.Application.UseCases.Users.Update;
using Sqids;

namespace MRB.Application.Configurations;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services, configuration);
        AddPasswordEncrypter(services, configuration);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 10,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });
        services.AddScoped(option =>
            new AutoMapper.MapperConfiguration(opt => { opt.AddProfile(new AutoMapping(sqids)); }).CreateMapper());
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
        services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
    }

    private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
    {
        //var additionalKey = configuration.GetSection("Settings:Password:AdditionalKey").Value;
        // var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        // services.AddScoped(options => new PasswordEncripter(additionalKey));
    }
}