using Microsoft.EntityFrameworkCore;

namespace JournalBackend.Models;

public class JournalDbContext : DbContext
{
    public JournalDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Entry> Entries { get; set; }

}