using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagerSite.Application.Data;
using UserManagerSite.Application.Models;

namespace UserManagerSite.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManagerSiteContext _context;

        public UserController(UserManagerSiteContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers(int? id, string? name, int? roleId)
        {
            var query = _context.User.Include(u => u.role).AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(u => u.id == id.Value);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.name.Contains(name));
            }
            if (roleId.HasValue)
            {
                query = query.Where(u => u.role.id == roleId.Value);
            }

            var users = await query.Select(u => new UserDTO
            {
                id = u.id,
                roleId = u.role.id ?? 0,
                roleName = u.role.role ?? "",
                name = u.name,
                email = u.email
            }).ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.User
                .Include(u => u.role)
                .Where(u => u.id == id)
                .Select(u => new UserDTO
                {
                    id = u.id,
                    roleId = u.role.id ?? 0,
                    roleName = u.role.role ?? "",
                    name = u.name,
                    email = u.email
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                var existingRole = await _context.Role.FindAsync(user.role.id);

                if (existingRole == null)
                {
                    return BadRequest("Role does not exist.");
                }

                user.role = existingRole;

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUser), new { id = user.id }, user);
            }

            return BadRequest(ModelState);
        }


        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRole = await _context.Role.FindAsync(user.role.id);

                    if (existingRole == null)
                    {
                        return BadRequest("Role does not exist.");
                    }

                    // Busca o usuário existente
                    var existingUser = await _context.User.FindAsync(id);

                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Atualiza os campos do usuário existente
                    existingUser.name = user.name;
                    existingUser.email = user.email;
                    existingUser.role = existingRole;

                    DateTime parsedBirthdate = new DateTime();
                    if (!string.IsNullOrEmpty(user.birthdate.ToString()))
                    {
                        // Tenta converter a string de data no formato "yyyy-MM-ddTHH:mm" para DateTime
                        if (DateTime.TryParseExact(user.birthdate.ToString(), "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                        {
                            parsedBirthdate = result;
                        }
                        else
                        {
                            return BadRequest($"Invalid birthdate format: '{user.birthdate}'. Please use 'yyyy-MM-ddTHH:mm' format.");
                        }
                    }

                    existingUser.birthdate = parsedBirthdate;

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.id == id);
        }
    }
}
