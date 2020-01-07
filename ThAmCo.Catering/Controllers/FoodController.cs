using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.Models;

namespace ThAmCo.Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public FoodController(CateringDbContext context) => _context = context;

        private List<FoodGetDto> GetDtos(List<Food> food) =>
            food.Select(x => GetDto(x)).ToList();

        private FoodGetDto GetDto(Food food) => 
            new FoodGetDto() { Id = food.Id, Cost = food.Cost, Name = food.Name };


        // GET: api/Foods
        [HttpGet]
        public async Task<List<FoodGetDto>> GetFood()
        {
            return GetDtos(await _context.Food.ToListAsync());
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFood([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var food = await _context.Food.FindAsync(id);

            if (food == null)
                return NotFound();

            return Ok(GetDto(food));
        }

        // PUT: api/Foods/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFood([FromRoute] int id, [FromBody] Food food)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != food.Id)
                return BadRequest();

            _context.Entry(food).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
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

        // POST: api/Foods
        [HttpPost]
        public async Task<IActionResult> PostFood([FromBody] Food food)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Food.Add(food);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFood", new { id = food.Id }, food);
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var food = await _context.Food.FindAsync(id);
            if (food == null)
                return NotFound();

            _context.Food.Remove(food);
            await _context.SaveChangesAsync();

            return Ok(food);
        }

        private bool FoodExists(int id)
        {
            return _context.Food.Any(e => e.Id == id);
        }
    }
}