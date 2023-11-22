using System.ComponentModel.DataAnnotations;

namespace JournalBackend.Models;

public class Entry
{
    [Required]
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public string? Content { get; set; }

}