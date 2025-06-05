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

// 🏗️ CREATE - Generate and save a new character
app.MapPost("/characters", async (FantasyDbContext db) =>
{
    var character = characterFaker.Generate();
    db.Characters.Add(character);
    await db.SaveChangesAsync();
    return Results.Created($"/characters/{character.Id}", character);
});

// 📖 READ - Get all characters from database
app.MapGet("/characters", async (FantasyDbContext db) =>
{
    return await db.Characters.ToListAsync();
});

// 📖 READ - Get specific character by ID
app.MapGet("/characters/{id}", async (int id, FantasyDbContext db) =>
{
    var character = await db.Characters.FindAsync(id);
    return character is not null ? Results.Ok(character) : Results.NotFound("Missing character");
});

// ✏️ UPDATE - Update an existing character
app.MapPut("/characters/{id}", async (int id, Character updatedCharacter, FantasyDbContext db) =>
{
    var character = await db.Characters.FindAsync(id);
    if (character is null) return Results.NotFound();

    character.Name = updatedCharacter.Name;
    character.Class = updatedCharacter.Class;
    character.Level = updatedCharacter.Level;
    character.Weapon = updatedCharacter.Weapon;

    await db.SaveChangesAsync();
    return Results.Ok(character);
});

// 🗡️ DELETE - Remove a character
app.MapDelete("/characters/{id}", async (int id, FantasyDbContext db) =>
{
    var character = await db.Characters.FindAsync(id);
    if (character is null) return Results.NotFound();

    db.Characters.Remove(character);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// 🎲 BONUS - Generate multiple characters at once
app.MapPost("/characters/batch/{count}", async (int count, FantasyDbContext db) =>
{
    var characters = characterFaker.Generate(count);
    db.Characters.AddRange(characters);
    await db.SaveChangesAsync();
    return Results.Created("/characters", characters);
});

app.Run();

