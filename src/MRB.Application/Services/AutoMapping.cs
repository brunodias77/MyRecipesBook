using AutoMapper;
using MRB.Communication.Requests.Recipes.Register;
using MRB.Communication.Requests.Users;
using MRB.Communication.Responses.Recipes;
using MRB.Communication.Responses.Users;
using MRB.Domain.Entities;
using MRB.Domain.Enums;
using Sqids;
using DishType = MRB.Domain.Entities.DishType;

namespace MRB.Application.Services;

public class AutoMapping : Profile
{
    private readonly SqidsEncoder<long> _idEncoder; // Declaração de uma variável privada para o encoder Sqids

    public AutoMapping(SqidsEncoder<long> idEncoder)
    {
        _idEncoder = idEncoder; // Inicialização do encoder Sqids no construtor

        RequestToDomain(); // Chamada de métodos para configurar mapeamentos de Request para Domain
        DomainToResponse(); // Chamada de métodos para configurar mapeamentos de Domain para Response
    }

    private void RequestToDomain()
    {
        // Configuração de mapeamento de RequestRegisterUserJson para User
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(user => user.Password, opt => opt.Ignore()); // Ignora o mapeamento da propriedade Password

        CreateMap<RequestRegisterRecipeJson, Recipe>()
            // Mapeamento para a coleção de Ingredients
            .ForMember(dest => dest.Ingredients,
                opt => opt.MapFrom(src => src.Ingredients.Select(i => new Ingredient { Item = i })))
            // Mapeamento para a coleção de Instructions
            .ForMember(dest => dest.Instructions,
                opt => opt.MapFrom(
                    src => src.Instructions.Select(i => new Instruction { Step = i.Step, Text = i.Text })))
            // Mapeamento para a coleção de DishTypes
            .ForMember(dest => dest.DishTypes,
                opt => opt.MapFrom(src =>
                    src.DishTypes.Select(d => new MRB.Domain.Entities.DishType { Type = (int)d })));
    }


    private void DomainToResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();
        CreateMap<Recipe, ResponseRegisteredRecipeJson>();
        CreateMap<Recipe, ResponseShortRecipeJson>()
            .ForMember(dest => dest.AmountIngredients, opt => opt.MapFrom(src => string.Join(", ", src.Ingredients.Select(i => i.Item))));
        CreateMap<Recipe, ResponseCompleteRecipeJson>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.CookingTime,
                opt => opt.MapFrom(src => src.CookingTime.HasValue ? (CookingTime?)src.CookingTime.Value : null))
            .ForMember(dest => dest.Difficulty,
                opt => opt.MapFrom(src => src.Difficulty.HasValue ? (Difficulty?)src.Difficulty.Value : null))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions))
            .ForMember(dest => dest.DishTypes,
                opt => opt.MapFrom(src => src.DishTypes.Select(d => (MRB.Domain.Enums.DishType)d.Type).ToList()));

        CreateMap<Ingredient, ResponseIngredientJson>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        CreateMap<Instruction, ResponseInstructionJson>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Step, opt => opt.MapFrom(src => src.Step))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text));
    }
}


// // Configuração de mapeamento de RequestRegisterRecipeJson para Recipe
// CreateMap<RequestRegisterRecipeJson, Recipe>()
//     .ForMember(dest => dest.Instructions,
//         opt => opt.Ignore()) // Ignora o mapeamento da propriedade Instructions
//     .ForMember(dest => dest.Ingredients,
//         opt => opt.MapFrom(source => source.Ingredients.Distinct())) // Mapeia Ingredients, aplicando Distinct
//     .ForMember(dest => dest.DishTypes,
//         opt => opt.MapFrom(source => source.DishTypes.Distinct())); // Mapeia DishTypes, aplicando Distinct
//
// // Configuração de mapeamento de string para Ingredient
// CreateMap<string, Ingredient>()
//     .ForMember(dest => dest.Item,
//         opt => opt.MapFrom(source => source)); // Mapeia string para a propriedade Item de Ingredient
//
// // Configuração de mapeamento de DishType (enum) para DishType (entidade)
// CreateMap<MRB.Domain.Enums.DishType, MRB.Domain.Entities.DishType>()
//     .ForMember(dest => dest.Type,
//         opt => opt.MapFrom(source =>
//             source)); // Mapeia DishType (enum) para a propriedade Type de DishType (entidade)
//
// // Configuração de mapeamento de RequestInstructionsJson para Instruction
// CreateMap<RequestInstructionsJson, Instruction>();