using MRB.Application.Configurations;
using MRB.Infra.Configurations;
using MRB.Infra.Extensions;
using MRB.Infra.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

app.Run();


void MigrateDatabase()
{
    if (builder.Configuration.IsUnitTestEnvironment())
        return;

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    DataBaseMigration.Migrate(builder.Configuration.ConnectionString(), serviceScope.ServiceProvider);
}

public partial class Program
{
    protected Program()
    {
    }
}