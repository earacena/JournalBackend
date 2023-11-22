using System.Data.Common;
using JournalBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace JournalBackend.Services;

public static class EntryService
{
    public static async Task<List<Entry>> GetAll(JournalDbContext db)
    {
        return await db.Entries.ToListAsync();
    }

    public static async Task<Entry?> Get(JournalDbContext db, string id)
    {
        return await db.Entries.FindAsync(id);
    }

    public static async void Add(JournalDbContext db, Entry entry)
    {
        entry.Id = Guid.NewGuid().ToString();
        await db.AddAsync(entry);
        await db.SaveChangesAsync();
    }

    public static async void Delete(JournalDbContext db, string id)
    {
        var entry = await Get(db, id);
        if (entry is null)
            return;

        db.Remove(entry);
        await db.SaveChangesAsync();
    }

    public static async void Update(JournalDbContext db, Entry entry)
    {
        var item = await Get(db, entry.Id);
        if (item is null)
            return;

        item.Content = entry.Content;
        await db.SaveChangesAsync();
    }
}