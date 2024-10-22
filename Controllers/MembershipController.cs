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
            .Include(m => m.User)                      // Incluir la relación con User
            .Include(m => m.TypesMembership)           // Incluir la relación con TypesMembership
            .ToListAsync();

        // Actualizar IsActive si EndDate ha pasado
        foreach (var membership in memberships)
        {
            if (membership.EndDate < DateTime.Now && membership.IsActive)
            {
                membership.IsActive = false;
            }
        }

        // Guardar los cambios en la base de datos si hubo modificaciones
        await _context.SaveChangesAsync();

        // Proyectar los resultados
        var result = memberships.Select(m => new
        {
            m.MembershipId,
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
            .Include(m => m.User)                      // Incluir la relación con User
            .Include(m => m.TypesMembership)           // Incluir la relación con TypesMembership
            .Where(m => m.UserId == userId)            // Filtrar por el UserId pasado en el endpoint
            .Select(m => new
            {
                m.MembershipId,
                UserName = m.User.Name,                // Usar el nombre del usuario en lugar de UserId
                MembershipType = m.TypesMembership.Name,  // Usar el nombre del tipo de membresía
                m.StartDate,
                m.EndDate,
                // Verificar si EndDate ha pasado y actualizar IsActive
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
        // Asignar la fecha actual a StartDate
        membership.StartDate = DateTime.Now;

        // Obtener la duración de TypesMembership y calcular EndDate
        var typesMembership = await _context.TypesMemberships
            .FirstOrDefaultAsync(tm => tm.Id == membership.TypesMembershipId);
        if (typesMembership == null)
        {
            return BadRequest(new { Message = "Invalid membership type." });
        }

        // Calcular EndDate basado en la duración
        membership.EndDate = membership.StartDate.AddDays((double)typesMembership.Duration);
        membership.IsActive = true;

        // Agregar la membresía y guardar en la base de datos
        _context.Memberships.Add(membership);
        await _context.SaveChangesAsync();

        // Obtener el nombre del usuario asociado al UserId
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == membership.UserId);
        if (user == null)
        {
            return BadRequest(new { Message = "User not found." });
        }

        // Devolver la información de la membresía junto con el nombre del usuario
        var result = new
        {
            membership.MembershipId,
            UserName = user.Name,  // Devolver el nombre del usuario
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
        // Buscar la entidad original en la base de datos
        var existingMembership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.MembershipId == id);

        if (existingMembership == null)
        {
            return NotFound(new { Message = "Membership not found" });
        }

        // Actualizar las propiedades solo si son válidas o no están vacías
        existingMembership.UserId = updatedMembership.UserId != 0 ? updatedMembership.UserId : existingMembership.UserId;
        existingMembership.TypesMembershipId = updatedMembership.TypesMembershipId != 0 ? updatedMembership.TypesMembershipId : existingMembership.TypesMembershipId;

        existingMembership.StartDate = updatedMembership.StartDate != DateTime.MinValue ? updatedMembership.StartDate : existingMembership.StartDate;
        existingMembership.EndDate = updatedMembership.EndDate != DateTime.MinValue ? updatedMembership.EndDate : existingMembership.EndDate;

        existingMembership.IsActive = updatedMembership.IsActive;

        // Guardar los cambios en la base de datos
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

    // Método auxiliar para comprobar si una membresía existe
    private bool MembershipExists(int id)
    {
        return _context.Memberships.Any(e => e.MembershipId == id);
    }

}
