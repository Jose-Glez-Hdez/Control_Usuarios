using System;
using Control_Usuarios.Context;
using Control_Usuarios.Models;
using Microsoft.AspNetCore.Mvc;

namespace Control_Usuarios.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccessController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    // GET: api/Access/user/{userId}
    [HttpGet("{userId}")]
    public ActionResult<Access> GetAccess(int userId)
    {
        var access = _context.Access.Find(userId);
        if (access == null)
        {
            return NotFound();
        }
        return access;
    }

    // POST: api/Access
    [HttpPost]
    public async Task<ActionResult<Access>> PostAccess(Access access)
    {
        access.DateAccess = DateTime.Now;
        _context.Access.Add(access);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetAccess", new { userId = access.UserId }, access);
    }
}
