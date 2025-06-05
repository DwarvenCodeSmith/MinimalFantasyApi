using MinimalFantasyApi.Configuration;
using MinimalFantasyApi.Data;
using MinimalFantasyApi.Endpoints;
using MinimalFantasyApi.Extensions;
using MinimalFantasyApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseErrorHandling();
app.UseStaticFiles();
app.UseHttpsRedirection();

// Map endpoints
app.MapCharacterEndpoints();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<FantasyDbContext>();
    context.Database.EnsureCreated();
}

app.Run();