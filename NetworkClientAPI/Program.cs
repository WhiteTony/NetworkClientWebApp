using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.NetworkInformation;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NetworkClientDb>(opt => opt.UseInMemoryDatabase("NetworkClientList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "0.0.1.0",
        Title = "NetworkClient API",
        Description = "API for managing a list of Network Clients and their power status.",
    });
});
// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5010") // Add your allowed origins here
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "NetworkClient API");
    });
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<NetworkClientDb>();
    dbContext.Database.EnsureCreated();
}

// Use CORS
app.UseCors();


// Network Client Mapping
app.MapGet("/networkclientlist",  async (NetworkClientDb db) =>
{
  return await db.NetworkClients.ToListAsync();
}).WithTags("Get all Network Clients");

app.MapGet("/networkclientlist/{id}/ping",  async (int id, NetworkClientDb db) =>
{
  NetworkClient? networkCLient = await db.NetworkClients.FindAsync(id);

  if (networkCLient == null)
  {
    return Results.NotFound();
  }
  if (networkCLient.Ip == null)
  {
    return Results.BadRequest("IP address is null.");
  }

  PingReply? pingReply = await networkCLient.PingAndProcessAsync();
  networkCLient.IsActive = pingReply.Status == IPStatus.Success;

  return Results.Ok(new {NetworkClient = networkCLient});
}
).WithTags("Ping a Network Client");

app.MapGet("/networkclientlist/active", async (NetworkClientDb db) =>
    await db.NetworkClients.Where(t => t.IsActive).ToListAsync())
    .WithTags("Get all the Network Clients that are active");

app.MapGet("/networkclientlist/{id}", async (int id, NetworkClientDb db) =>
    await db.NetworkClients.FindAsync(id)
        is NetworkClient networkClient
            ? Results.Ok(networkClient)
            : Results.NotFound())
    .WithTags("Get Network Client by Id");

app.MapPost("/networkclientlist", async (NetworkClient networkClient, NetworkClientDb db) =>
{
    db.NetworkClients.Add(networkClient);
    await db.SaveChangesAsync();

    return Results.Created($"/networkclientlist/{networkClient.Id}", networkClient);
})
    .WithTags("Add Network Client to list");

app.MapPut("/networkclientlist/{id}", async (int id, NetworkClient inputNetworkClient, NetworkClientDb db) =>
{
    var networkClient = await db.NetworkClients.FindAsync(id);

    if (networkClient is null) return Results.NotFound();

    networkClient.Name = inputNetworkClient.Name;
    networkClient.Ip = inputNetworkClient.Ip;
    networkClient.IsActive = inputNetworkClient.IsActive;

    await db.SaveChangesAsync();

    return Results.NoContent();
})
    .WithTags("Update Network Client by Id");

app.MapDelete("/networkclientlist/{id}", async (int id, NetworkClientDb db) =>
{
    if (await db.NetworkClients.FindAsync(id) is NetworkClient networkClient)
    {
        db.NetworkClients.Remove(networkClient);
        await db.SaveChangesAsync();
        return Results.Ok(networkClient);
    }

    return Results.NotFound();
})
    .WithTags("Delete Network Client by Id");





app.Run();



