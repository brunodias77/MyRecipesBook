using AutoMapper;
using MRB.Communication.Requests.Instructions;
using MRB.Communication.Requests.Recipes.Register;
using MRB.Communication.Requests.Users;
using MRB.Communication.Responses.Recipes;
using MRB.Communication.Responses.Users;
using MRB.Domain.Entities;
using Sqids;

namespace MRB.Application.Services;

public class AutoMapping : Profile
{
    private readonly SqidsEncoder<long> _idEncoder;

    public AutoMapping(SqidsEncoder<long> idEncoder)
    {
        _idEncoder = idEncoder;
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(user => user.Password, opt => opt.Ignore());
        CreateMap<RequestRegisterRecipeJson, Recipe>()
            .ForMember(dest => dest.Instructions, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));
        CreateMap<string, Ingredient>()
            .ForMember(dest => dest.Item, opt => opt.MapFrom(source => source));
        CreateMap<MRB.Domain.Enums.DishType, MRB.Domain.Entities.DishType>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));
        CreateMap<RequestInstructionsJson, Instruction>();
    }

    private void DomainToResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();
        // CreateMap<Recipe, ResponseRegisteredRecipeJson>()
        // .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEncoder.Encode(source.Id)));
    }
}