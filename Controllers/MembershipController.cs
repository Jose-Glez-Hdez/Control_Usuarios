using System;
using Control_Usuarios.Context;
using Control_Usuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Control_Usuarios.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MembershipController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    // GET: api/Membership
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetMemberships()
    {
        var memberships = await _context.Memberships
            .Include(m => m.User)                      // Include the relation with User.
            .Include(m => m.TypesMembership)           // Include the relation with TypesMembership.
            .ToListAsync();

        foreach (var membership in memberships)
        {
            if (membership.EndDate < DateTime.Now && membership.IsActive)
            {
                membership.IsActive = false;
            }
        }

        await _context.SaveChangesAsync();

        var result = memberships.Select(m => new
        {
            m.Id,
            UserName = m.User.Name,
            MembershipType = m.TypesMembership.Name,
            m.StartDate,
            m.EndDate,
            m.IsActive
        });

        return Ok(result);
    }

    // GET: api/Membership/user/{userId}
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<object>>> GetMembershipsByUser(int userId)
    {
        var memberships = await _context.Memberships
            .Include(m => m.User)
            .Include(m => m.TypesMembership)
            .Where(m => m.UserId == userId)
            .Select(m => new
            {
                m.Id,
                UserName = m.User.Name,                // Use the name of the user instead of UserId.
                MembershipType = m.TypesMembership.Name,  // Use the name of the membership type instead of TypesMembershipId.
                m.StartDate,
                m.EndDate,
                IsActive = m.EndDate < DateTime.Now ? false : m.IsActive
            })
            .ToListAsync();

        if (!memberships.Any())
        {
            return NotFound(new { Message = $"No memberships found for User ID {userId}" });
        }

        return Ok(memberships);
    }

    // POST: api/Membership
    [HttpPost]
    public async Task<ActionResult<object>> PostMembership(Membership membership)
    {
        membership.StartDate = DateTime.Now;

        var typesMembership = await _context.TypesMemberships
            .FirstOrDefaultAsync(tm => tm.Id == membership.TypesMembershipId);
        if (typesMembership == null)
        {
            return BadRequest(new { Message = "Invalid membership type." });
        }

        membership.EndDate = membership.StartDate.AddDays((double)typesMembership.Duration);
        membership.IsActive = true;

        _context.Memberships.Add(membership);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == membership.UserId);
        if (user == null)
        {
            return BadRequest(new { Message = "User not found." });
        }

        var result = new
        {
            membership.Id,
            UserName = user.Name, 
            MembershipType = typesMembership.Name,
            membership.StartDate,
            membership.EndDate,
            membership.IsActive
        };

        return CreatedAtAction(nameof(GetMembershipsByUser), new { userId = membership.UserId }, result);
    }

    // DELETE: api/Membership/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<Membership>> DeleteMembership(int id)
    {
        var membership = await _context.Memberships.FindAsync(id);
        if (membership == null) return NotFound("Membership not found.");
        _context.Memberships.Remove(membership);
        await _context.SaveChangesAsync();
        return Ok(membership);
    }

    // PUT: api/Membership/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMembership(int id, Membership updatedMembership)
    {
        var existingMembership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.Id == id);

        if (existingMembership == null)
        {
            return NotFound(new { Message = "Membership not found" });
        }

        UpdateMembership(existingMembership, updatedMembership);
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MembershipExists(id))
            {
                return NotFound(new { Message = "Membership not found after update" });
            }
            else
            {
                throw;
            }
        }

        return Ok(existingMembership);
    }

    private void UpdateMembership(Membership existingMembership, Membership updatedMembership)
    {
        existingMembership.UserId = updatedMembership.UserId != 0 ? updatedMembership.UserId : existingMembership.UserId;
        existingMembership.TypesMembershipId = updatedMembership.TypesMembershipId != 0 ? updatedMembership.TypesMembershipId : existingMembership.TypesMembershipId;

        existingMembership.StartDate = updatedMembership.StartDate != DateTime.MinValue ? updatedMembership.StartDate : existingMembership.StartDate;
        existingMembership.EndDate = updatedMembership.EndDate != DateTime.MinValue ? updatedMembership.EndDate : existingMembership.EndDate;

        existingMembership.IsActive = updatedMembership.IsActive;
    }

    // Método auxiliar para comprobar si una membresía existe
    private bool MembershipExists(int id)
    {
        return _context.Memberships.Any(e => e.Id == id);
    }

}
