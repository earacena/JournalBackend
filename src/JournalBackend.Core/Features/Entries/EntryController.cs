using Microsoft.AspNetCore.Mvc;
using JournalBackend.Models;
using JournalBackend.Services;

namespace JournalBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntryController : ControllerBase
{
    private static readonly List<Entry> Entries = new()
    {
        new() {
            Id = "entry1",
            UserId = "abcd1",
            Date = DateTime.Now,
        }
    };

    private readonly ILogger<EntryController> _logger;
    
    public EntryController(ILogger<EntryController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<List<Entry>> GetAll()
    {
        return Ok(EntryService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<Entry> Get(string id)
    {
        var entry = EntryService.Get(id);

        if (entry is null)
            return NotFound();
        
        return Ok(entry);
    }

    [HttpPost]
    public IActionResult Create(Entry entry)
    {
        EntryService.Add(entry);
        return CreatedAtAction(nameof(Get), new  { Id = entry.Id }, entry);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, Entry entry)
    {
        if (id != entry.Id)
            return BadRequest();
        
        var existingEntry = EntryService.Get(id);
        if (existingEntry is null)
            return NotFound();

        EntryService.Update(entry);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var entry = EntryService.Get(id);
        if (entry is null) 
            return NotFound();

        EntryService.Delete(id);
        return NoContent();
    }
}