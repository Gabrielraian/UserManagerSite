using System;
using System.Collections.Generic;
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
    public class RoleController : ControllerBase
    {
        private readonly UserManagerSiteContext _context;

        public RoleController(UserManagerSiteContext context)
        {
            _context = context;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles(int? id, string? role)
        {
            var query = _context.Role.AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(r => r.id == id.Value);
            }

            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(r => r.role.Contains(role));
            }

            return await query.ToListAsync();
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Role.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // POST: api/Role
        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Role.Add(role);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetRole), new { id = role.id }, role);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditRole(int id, Role role)
        {
            if (id != role.id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(role).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(id))
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

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Role.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return _context.Role.Any(e => e.id == id);
        }
    }
}
