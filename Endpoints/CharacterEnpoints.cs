using MinimalFantasyApi.Models;
using MinimalFantasyApi.Services;

namespace MinimalFantasyApi.Endpoints;

public static class CharacterEndpoints
{
    public static void MapCharacterEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/characters", async (CharacterService characterService) =>
            await characterService.GetAllAsync())
            .WithName("GetAllCharacters")
            .WithDescription("Gets all fantasy characters")
            .WithOpenApi();

        app.MapGet("/characters/{id}", async (int id, CharacterService characterService) =>
        {
            var character = await characterService.GetByIdAsync(id);
            return character is not null
                ? Results.Ok(character)
                : Results.NotFound();
        })
            .WithName("GetCharacterById")
            .WithDescription("Gets a fantasy character by ID")
            .WithOpenApi();

        app.MapPost("/characters", async (Character character, CharacterService characterService) =>
        {
            var createdCharacter = await characterService.CreateAsync(character);
            return Results.Created($"/characters/{createdCharacter.Id}", createdCharacter);
        })
            .WithName("CreateCharacter")
            .WithDescription("Creates a new fantasy character")
            .WithOpenApi();

        app.MapPut("/characters/{id}", async (int id, Character character, CharacterService characterService) =>
        {
            var updatedCharacter = await characterService.UpdateAsync(id, character);
            return updatedCharacter is not null
                ? Results.Ok(updatedCharacter)
                : Results.NotFound();
        })
            .WithName("UpdateCharacter")
            .WithDescription("Updates a fantasy character")
            .WithOpenApi();

        app.MapDelete("/characters/{id}", async (int id, CharacterService characterService) =>
        {
            var deleted = await characterService.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        })
            .WithName("DeleteCharacter")
            .WithDescription("Deletes a fantasy character")
            .WithOpenApi();

        app.MapPost("/characters/batch/{count}", async (int count, CharacterService characterService) =>
        {
            var characters = await characterService.GenerateRandomCharactersAsync(count);
            return Results.Ok(characters);
        })
            .WithName("GenerateRandomCharacters")
            .WithDescription("Generates multiple random fantasy characters")
            .WithOpenApi();
    }
}