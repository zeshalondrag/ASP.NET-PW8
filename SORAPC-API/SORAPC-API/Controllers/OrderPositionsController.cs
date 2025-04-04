using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC_API.Models;

namespace SORAPC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Администратор")]
    public class OrderPositionsController : ControllerBase
    {
        private readonly SorapcContext _context;

        public OrderPositionsController(SorapcContext context)
        {
            _context = context;
        }

        // GET: api/OrderPositions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderPosition>>> GetOrderPositions()
        {
            return await _context.OrderPositions.ToListAsync();
        }

        // GET: api/OrderPositions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderPosition>> GetOrderPosition(int? id)
        {
            var orderPosition = await _context.OrderPositions.FindAsync(id);

            if (orderPosition == null)
            {
                return NotFound();
            }

            return orderPosition;
        }

        // PUT: api/OrderPositions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderPosition(int? id, OrderPosition orderPosition)
        {
            if (id != orderPosition.IdOrderPosition)
            {
                return BadRequest();
            }

            _context.Entry(orderPosition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderPositionExists(id))
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

        // POST: api/OrderPositions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderPosition>> PostOrderPosition(OrderPosition orderPosition)
        {
            _context.OrderPositions.Add(orderPosition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderPosition", new { id = orderPosition.IdOrderPosition }, orderPosition);
        }

        // DELETE: api/OrderPositions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderPosition(int? id)
        {
            var orderPosition = await _context.OrderPositions.FindAsync(id);
            if (orderPosition == null)
            {
                return NotFound();
            }

            _context.OrderPositions.Remove(orderPosition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderPositionExists(int? id)
        {
            return _context.OrderPositions.Any(e => e.IdOrderPosition == id);
        }
    }
}
