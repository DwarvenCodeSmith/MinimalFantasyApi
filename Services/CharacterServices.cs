using Bogus;
using Microsoft.EntityFrameworkCore;
using MinimalFantasyApi.Data;
using MinimalFantasyApi.Models;

namespace MinimalFantasyApi.Services;

public class CharacterService
{
    private readonly FantasyDbContext _db;

    public CharacterService(FantasyDbContext db)
    {
        _db = db;
    }

    public async Task<List<Character>> GetAllAsync()
    {
        return await _db.Characters.ToListAsync();
    }

    public async Task<Character?> GetByIdAsync(int id)
    {
        return await _db.Characters.FindAsync(id);
    }

    public async Task<Character> CreateAsync(Character character)
    {
        _db.Characters.Add(character);
        await _db.SaveChangesAsync();
        return character;
    }

    public async Task<Character?> UpdateAsync(int id, Character character)
    {
        var existingCharacter = await _db.Characters.FindAsync(id);
        if (existingCharacter == null) return null;
        
        existingCharacter.Name = character.Name;
        existingCharacter.Class = character.Class;
        existingCharacter.Level = character.Level;
        existingCharacter.Weapon = character.Weapon;
        
        await _db.SaveChangesAsync();
        return existingCharacter;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var character = await _db.Characters.FindAsync(id);
        if (character == null) return false;
        
        _db.Characters.Remove(character);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Character>> GenerateRandomCharactersAsync(int count)
    {
        var faker = new Faker<Character>()
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.Class, f => f.PickRandom("Warrior", "Wizard", "Rogue", "Cleric", "Paladin"))
            .RuleFor(c => c.Level, f => f.Random.Number(1, 20))
            .RuleFor(c => c.Weapon, f => f.PickRandom("Sword", "Staff", "Daggers", "Mace", "Bow"));

        var characters = faker.Generate(count);
        _db.Characters.AddRange(characters);
        await _db.SaveChangesAsync();
        
        return characters;
    }
}