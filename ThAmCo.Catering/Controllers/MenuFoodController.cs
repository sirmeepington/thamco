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
    public class MenuFoodController : ControllerBase
    {

        private readonly CateringDbContext _context;

        public MenuFoodController(CateringDbContext context) => _context = context;

        // GET: /api/menufood/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIdsFromMenu(int id)
        {

            Menu menu = await _context.Menus.Include(m => m.Food).FirstOrDefaultAsync(x => x.Id == id);
            if (menu == null)
                return NotFound();

            List<MenuFoodGetDto> outList = menu.Food.Select(x => new MenuFoodGetDto { FoodId = x.FoodId, MenuId = x.MenuId }).ToList();
            return Ok(outList);
        }

        // POST: /api/menufood/1/1 
        [HttpPost]
        public async Task<IActionResult> AddFoodToMenu(int menuId, int foodId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Menu menu = await _context.Menus.FindAsync(menuId);
            Food food = await _context.Food.FindAsync(foodId);
            if (menu == null || food == null)
                return NotFound();

            MenuFood mf = new MenuFood() { FoodId = foodId, MenuId = menuId, Food = food, Menu = menu };

            await _context.MenuFood.AddAsync(mf);
            await _context.SaveChangesAsync();

            return Ok(new MenuFoodGetDto() { FoodId = mf.FoodId, MenuId = mf.MenuId });
        }

        // DELETE: /api/menufood/1/1 
        [HttpDelete("{menuId}/{foodId}")]
        public async Task<IActionResult> RemoveFoodFromMenu(int menuId, int foodId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            MenuFood mf = await _context.MenuFood.FindAsync(menuId, foodId);
            if (mf == null)
                return NotFound();

            _context.MenuFood.Remove(mf);
            await _context.SaveChangesAsync();

            return Ok();
            
        }
    }
}