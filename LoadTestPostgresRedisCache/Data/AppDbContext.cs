using LoadTestPostgresRedisCache.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LoadTestPostgresRedisCache.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
