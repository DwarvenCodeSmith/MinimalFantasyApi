namespace MinimalFantasyApi.Models;
public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Weapon { get; set; } = string.Empty;
}