using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Control_Usuarios.Context;
using Control_Usuarios.Models;
using System.Reflection.Metadata.Ecma335;

namespace Control_Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/{Id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user != null ? user : NotFound("User not found.");
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(); 
            try
            {
                var typesMembership = await _context.TypesMemberships
                    .FirstOrDefaultAsync(tm => tm.Id == user.MembershipId);
                if (typesMembership == null)
                {
                    return BadRequest(new { Message = "Invalid membership type." });
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                var membership = new Membership
                {
                    UserId = user.Id,
                    TypesMembershipId = user.MembershipId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(typesMembership.Duration),
                    IsActive = true
                };

                _context.Memberships.Add(membership);
                await _context.SaveChangesAsync(); 
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User newUserData)
        {

            if (newUserData == null) return NotFound("User data is null");
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found");

            SetDefaultValues(newUserData);

            UpdateValues(user, newUserData);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }


        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(User);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(user => user.Id == id);
        }

        private void SetDefaultValues(User user)
        {
            user.Name ??= string.Empty;
            user.Surname ??= string.Empty;
            user.Dni ??= string.Empty;
            user.Email ??= string.Empty;
            user.Address ??= string.Empty;
            user.Phone ??= string.Empty;
            user.MembershipId = user.MembershipId < 0 ? 0 : user.MembershipId;
            user.Birthdate = user.Birthdate == default ? DateOnly.MinValue : user.Birthdate;
        }
        private void UpdateValues(User user, User newUserData)
        {
            user.Name = !string.IsNullOrEmpty(newUserData.Name) ? newUserData.Name : user.Name;
            user.Surname = !string.IsNullOrEmpty(newUserData.Surname) ? newUserData.Surname : user.Surname;
            user.Dni = !string.IsNullOrEmpty(newUserData.Dni) ? newUserData.Dni : user.Dni;
            user.Email = !string.IsNullOrEmpty(newUserData.Email) ? newUserData.Email : user.Email;
            user.Address = !string.IsNullOrEmpty(newUserData.Address) ? newUserData.Address : user.Address;
            user.Phone = !string.IsNullOrEmpty(newUserData.Phone) ? newUserData.Phone : user.Phone;
            user.MembershipId = newUserData.MembershipId < 0 ? 0 : newUserData.MembershipId;
            user.Birthdate = newUserData.Birthdate != default ? newUserData.Birthdate : user.Birthdate;
        }
    }
}
