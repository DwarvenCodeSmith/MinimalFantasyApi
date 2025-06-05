using Bogus;
using Microsoft.EntityFrameworkCore;
using MinimalFantasyApi.Data;
using MinimalFantasyApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FantasyDbContext>(options =>
    options.UseSqlite("Data Source=fantasy.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FantasyDbContext>();
    db.Database.EnsureCreated();
}

var characterFaker = new Faker<Character>()
    .RuleFor(c => c.Name, f => $"{f.Name.FirstName()} {f.Name.LastName()}")
    .RuleFor(c => c.Class, f => f.PickRandom("Warrior", "Mage", "Rogue", "Cleric", "Ranger"))
    .RuleFor(c => c.Level, f => f.Random.Int(1, 20))
    .RuleFor(c => c.Weapon, f => f.PickRandom("Longsword", "Great Axe", "Staff", "Dagger", "Bow"));

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/characters", () =>
{
    return characterFaker.Generate(1);
});

app.Run();

