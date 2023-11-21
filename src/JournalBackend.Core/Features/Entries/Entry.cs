namespace JournalBackend.Models;

public class Entry
{
    public required String Id { get; set; }
    public DateTime Date { get; set; }
    public required String UserId { get; set; }
    public String? Content { get; set; }

}