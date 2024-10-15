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
            return user == null ? NotFound() : user;
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, User);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User newUserData)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) return NotFound();

            //Update user data if newUserData is not null
            user.Name = string.IsNullOrEmpty(newUserData.Name)? user.Name : newUserData.Name;
            user.Surname = string.IsNullOrEmpty(newUserData.Surname)? user.Surname : newUserData.Surname;
            user.Email = string.IsNullOrEmpty(newUserData.Email)? user.Email : newUserData.Email;
            user.Address = string.IsNullOrEmpty(newUserData.Address)? user.Address : newUserData.Address;
            user.Phone = string.IsNullOrEmpty(newUserData.Phone)? user.Phone : newUserData.Phone;
            user.Birthdate = newUserData.Birthdate != default ? user.Birthdate : newUserData.Birthdate;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
