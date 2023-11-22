using JournalBackend.Models;

namespace JournalBackend.Services;

public static class EntryService
{
    static List<Entry> Entries { get; }

    static EntryService()
    {
        Entries = new List<Entry>()
        {
            new() {
                Id = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                Content = "This is content for a journal entry."
            },

            new() {
                Id = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                Content = "This is content for a journal entry."
            }
        };
    }

    public static List<Entry> GetAll() => Entries;

    public static Entry? Get(string id)
    {
        return Entries.FirstOrDefault(e => e.Id == id);
    }

    public static void Add(Entry entry)
    {
        entry.Id = Guid.NewGuid().ToString();
        Entries.Add(entry);
    }

    public static void Delete(string id)
    {
        var entry = Get(id);
        if (entry is null)
            return;

        Entries.Remove(entry);
    }

    public static void Update(Entry entry)
    {
        var index = Entries.FindIndex(e => e.Id == entry.Id);
        
        if (index == -1)
            return;

        Entries[index] = entry;
    }
}