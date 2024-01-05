using Microsoft.AspNetCore.Mvc;
using JournalBackend.Models;
using JournalBackend.Services;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<Entry>>> GetAll(JournalDbContext db)
    {
        string? role = User.Claims.FirstOrDefault(claim => claim.Type.Equals("role"))?.Value;
        
        if (role == "admin") {
          var allEntries = await EntryService.GetAll(db);
          return Ok(allEntries);
        } else {
            return Unauthorized();
        }
    }

    [Authorize]
    [HttpGet("User/{userId}")]
    public async Task<ActionResult<List<Entry>>> GetAllOfUser(JournalDbContext db, string userId)
    {
        string? authUserId = User.Claims.FirstOrDefault(claim => claim.Type.Equals("user_id", StringComparison.OrdinalIgnoreCase))?.Value;

        if (userId != authUserId) {
            return Unauthorized();
        }

        var allUserEntries = await EntryService.GetAllOfUser(db, userId);
        return Ok(allUserEntries);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Entry>> Get(JournalDbContext db, string id)
    {
        string? userId = User.Claims.FirstOrDefault(claim => claim.Type.Equals("user_id", StringComparison.OrdinalIgnoreCase))?.Value;
        
        var entry = await EntryService.Get(db, id);

        if (entry is null){
            return NotFound();
        }

        if (entry.UserId != userId) {
            return Unauthorized();
        }

        return Ok(entry);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create(JournalDbContext db, Entry entry)
    {
        // Access userId 
        string? userId = User.Claims.FirstOrDefault(claim => claim.Type.Equals("user_id", StringComparison.OrdinalIgnoreCase))?.Value;

        if (userId is null) {
            return Unauthorized();
        }

        entry.UserId = userId;
        EntryService.Add(db, entry);
        return CreatedAtAction(nameof(Get), new { entry.Id }, entry);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(JournalDbContext db, string id, Entry entry)
    {
        string? userId = User.Claims.FirstOrDefault(claim => claim.Type.Equals("user_id", StringComparison.OrdinalIgnoreCase))?.Value;

        if (id != entry.Id)
            return BadRequest();
        
        var existingEntry = await EntryService.Get(db, id);
        if (existingEntry is null)
            return NotFound();

        if (entry.UserId != userId) {
            return Unauthorized();
        }

        EntryService.Update(db, entry);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(JournalDbContext db, string id)
    {
        string? userId = User.Claims.FirstOrDefault(claim => claim.Type.Equals("user_id", StringComparison.OrdinalIgnoreCase))?.Value;

        var entry = await EntryService.Get(db, id);
        if (entry is null) 
            return NotFound();

        if (entry.UserId != userId) {
            return Unauthorized();
        }

        EntryService.Delete(db, id);
        return NoContent();
    }
}