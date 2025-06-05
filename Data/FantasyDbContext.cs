using Microsoft.EntityFrameworkCore;
using MinimalFantasyApi.Models;

namespace MinimalFantasyApi.Data;

public class FantasyDbContext : DbContext
{
    public FantasyDbContext(DbContextOptions<FantasyDbContext> options) : base(options) { }

    public DbSet<Character> Characters => Set<Character>();
}
