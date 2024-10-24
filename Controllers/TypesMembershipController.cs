using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Control_Usuarios.Context;
using Control_Usuarios.Models;

namespace Control_Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesMembershipController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/TypeMembership
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypesMembership>>> GetTypesMemberships()
        {
            return await _context.TypesMemberships.ToListAsync();
        }

        // GET: api/TypesMembership/{Id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TypesMembership>> GetTypesMembership(int id)
        {
            var typesMembership = await _context.TypesMemberships.FindAsync(id);
            return typesMembership != null ? typesMembership : NotFound("TypesMembership not found.");
        }

        // POST: api/TypesMembership
        [HttpPost]
        public async Task<ActionResult<TypesMembership>> PostTypesMembership(TypesMembership typesMembership)
        {
            _context.TypesMemberships.Add(typesMembership);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTypesMembership", new { id = typesMembership.Id }, typesMembership);
        }

        // PUT: api/TypesMembership/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypesMembership(int id, TypesMembership newTypeMembership)
        {
            if (newTypeMembership == null) return NotFound("User data is null.");
            var typeMembership = await _context.TypesMemberships.FindAsync(id);
            if (typeMembership == null) return NotFound("Type of membership not found.");

            SetDefaultValues(newTypeMembership);


            UpdateValues(typeMembership, newTypeMembership);

            _context.TypesMemberships.Update(typeMembership);
            await _context.SaveChangesAsync();
            return Ok(typeMembership);
        }

        // DELETE: api/TypesMembership/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypesMembership>> DeleteTypesMembership(int id)
        {
            var typeMembership = await _context.TypesMemberships.FindAsync(id);
            if (typeMembership == null) return NotFound("Type of membership not found.");
            _context.TypesMemberships.Remove(typeMembership);
            await _context.SaveChangesAsync();
            return Ok(typeMembership);
        }

        private void UpdateValues(TypesMembership typeMembership, TypesMembership newTypeMembership)
        {
            typeMembership.Name = !string.IsNullOrEmpty(newTypeMembership.Name)? newTypeMembership.Name : typeMembership.Name;
            typeMembership.Info =!string.IsNullOrEmpty(newTypeMembership.Info)? newTypeMembership.Info : typeMembership.Info;
            typeMembership.Price = typeMembership.Price != default? newTypeMembership.Price : typeMembership.Price;
            typeMembership.Duration = typeMembership.Duration!= default? newTypeMembership.Duration : typeMembership.Duration;
        }

        private void SetDefaultValues(TypesMembership typeMembership)
        {
            typeMembership.Name ??= string.Empty;
            typeMembership.Info ??= string.Empty;
            typeMembership.Price = typeMembership.Price < 0? 0 : typeMembership.Price;
            typeMembership.Duration = typeMembership.Duration < 0? 0 : typeMembership.Duration;
        }
    }
}
    

