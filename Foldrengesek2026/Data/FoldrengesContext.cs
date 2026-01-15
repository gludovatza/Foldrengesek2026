using Microsoft.EntityFrameworkCore;

namespace Foldrengesek2026.Data
{
    public class FoldrengesContext : DbContext
    {
        public DbSet<Models.Telepules> Telepulesek { get; set; } = null!;
        public DbSet<Models.Naplo> Naplok { get; set; } = null!;

        public FoldrengesContext(DbContextOptions<FoldrengesContext> options) : base(options)
        {
        }
    }
}
