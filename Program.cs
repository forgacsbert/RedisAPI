using RedisAPI.Data;
using RedisAPI.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!)
);

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/platforms", (IPlatformRepository repository) =>
{
    return Results.Ok(repository.GetPlatforms());
}).Produces<IEnumerable<Platform>>();

app.MapGet("/platforms/{id}", (IPlatformRepository repository, string id) =>
{
    var platform = repository.GetPlatform(id);

    return platform is not null ? Results.Ok(platform) : Results.NotFound();
}).Produces<Platform>();

app.MapPost("/platforms", (IPlatformRepository repository, Platform platform) =>
{
    repository.CreatePlatform(platform);

    return Results.Created($"/platforms/{platform.Id}", platform);
}).Produces<Platform>();

app.MapDelete("/platforms/{id}", (IPlatformRepository repository, string id) =>
{
    repository.DeletePlatform(id);

    return Results.NoContent();
});

app.Run();