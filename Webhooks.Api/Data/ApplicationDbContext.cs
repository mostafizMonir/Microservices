using Microsoft.EntityFrameworkCore;
using Webhooks.Api.Models;

namespace Webhooks.Api.Data;

public class ApplicationDbContext:DbContext
{
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Order> Orders { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

}
