using System.ComponentModel.DataAnnotations;

namespace JournalBackend.Models;

public class Entry
{
    public string Id { get; set; } = System.Guid.NewGuid().ToString();
    public DateTime Date { get; set; } = DateTime.Now;

    [Required]
    public string UserId { get; set; } = string.Empty;
    public string? Content { get; set; }

}