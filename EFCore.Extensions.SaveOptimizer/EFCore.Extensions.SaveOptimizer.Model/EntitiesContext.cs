using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Model;

public class EntitiesContext : DbContext
{
    public DbSet<NonRelatedEntity>? NonRelatedEntities { get; set; }
}
