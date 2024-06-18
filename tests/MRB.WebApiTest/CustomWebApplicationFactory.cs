using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRB.CommonTest.Entities;
using MRB.Domain.Entities;
using MRB.Infra.Data;

namespace MRB.WebApiTest;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private User _user = default!;
    private string _password = string.Empty;

    // Sobrescreve o método para configurar o host da aplicação web
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Define o ambiente como "Test" para a aplicação web
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                // Remove a configuração existente do DbContextOptions<MyRecipesDbContext> se existir
                var descriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                // Configura um provedor de serviços para o banco de dados em memória
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                // Configura o DbContext usando um banco de dados em memória para testes
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting"); // Nome do banco de dados em memória
                    options.UseInternalServiceProvider(provider); // Usa o provedor de serviços configurado
                });

                // Cria um escopo de serviço para interagir com o DbContext
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Garante que o banco de dados em memória está vazio antes de iniciar
                dbContext.Database.EnsureDeleted();

                // Inicia o banco de dados em memória com dados de teste
                StartDataBase(dbContext);
            });
    }

    // Método privado para iniciar o banco de dados com dados de teste
    private void StartDataBase(ApplicationDbContext dbContext)
    {
        // Usa um builder de entidades (supondo UserBuilder) para construir um usuário de teste
        (_user, _password) = UserBuilder.Build();

        // Adiciona o usuário ao contexto do banco de dados em memória e salva mudanças
        dbContext.Users.Add(_user);
        dbContext.SaveChanges();
    }

    // Métodos públicos para obter informações do usuário de teste
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public Guid GetUserId() => _user.Id;
}