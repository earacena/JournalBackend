using Microsoft.AspNetCore.Mvc;
using JournalBackend.Models;
using JournalBackend.Services;

namespace JournalBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntryController : ControllerBase
{

    private readonly ILogger<EntryController> _logger;
    
    public EntryController(ILogger<EntryController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Entry>>> GetAll(JournalDbContext db)
    {
        var allEntries = await EntryService.GetAll(db);
        return Ok(allEntries);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Entry>> Get(JournalDbContext db, string id)
    {
        var entry = await EntryService.Get(db, id);

        if (entry is null)
            return NotFound();
        
        return Ok(entry);
    }

    [HttpPost]
    public IActionResult Create(JournalDbContext db, Entry entry)
    {
        EntryService.Add(db, entry);
        return CreatedAtAction(nameof(Get), new { entry.Id }, entry);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(JournalDbContext db, string id, Entry entry)
    {
        if (id != entry.Id)
            return BadRequest();
        
        var existingEntry = await EntryService.Get(db, id);
        if (existingEntry is null)
            return NotFound();

        EntryService.Update(db, entry);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(JournalDbContext db, string id)
    {
        var entry = EntryService.Get(db, id);
        if (entry is null) 
            return NotFound();

        EntryService.Delete(db, id);
        return NoContent();
    }
}