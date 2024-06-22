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

        // Configuração de mapeamento de RequestRegisterRecipeJson para Recipe
        CreateMap<RequestRegisterRecipeJson, Recipe>()
            .ForMember(dest => dest.Instructions,
                opt => opt.Ignore()) // Ignora o mapeamento da propriedade Instructions
            .ForMember(dest => dest.Ingredients,
                opt => opt.MapFrom(source => source.Ingredients.Distinct())) // Mapeia Ingredients, aplicando Distinct
            .ForMember(dest => dest.DishTypes,
                opt => opt.MapFrom(source => source.DishTypes.Distinct())); // Mapeia DishTypes, aplicando Distinct

        // Configuração de mapeamento de string para Ingredient
        CreateMap<string, Ingredient>()
            .ForMember(dest => dest.Item,
                opt => opt.MapFrom(source => source)); // Mapeia string para a propriedade Item de Ingredient

        // Configuração de mapeamento de DishType (enum) para DishType (entidade)
        CreateMap<MRB.Domain.Enums.DishType, MRB.Domain.Entities.DishType>()
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(source =>
                    source)); // Mapeia DishType (enum) para a propriedade Type de DishType (entidade)

        // Configuração de mapeamento de RequestInstructionsJson para Instruction
        CreateMap<RequestInstructionsJson, Instruction>();
    }

    private void DomainToResponse()
    {
        // Configuração de mapeamento de User para ResponseUserProfileJson
        CreateMap<User, ResponseUserProfileJson>();

        // Exemplo de configuração de mapeamento de Recipe para ResponseRegisteredRecipeJson
        // Observação: Este mapeamento está comentado, possivelmente para futura implementação
        // CreateMap<Recipe, ResponseRegisteredRecipeJson>()
        //     .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEncoder.Encode(source.Id)));
        // A linha acima mapeia o Id de Recipe para ResponseRegisteredRecipeJson usando o encoder Sqids para codificar o Id.
    }
}