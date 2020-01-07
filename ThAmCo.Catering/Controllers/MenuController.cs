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
    public class MenuController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public MenuController(CateringDbContext context)
        {
            _context = context;
        }

        private List<MenuGetDto> GetMenus()
        {
            List<MenuGetDto> menus = _context.Menus.Include(x => x.Food).Select(x =>
            new MenuGetDto
            {
                Id = x.Id,
                Name = x.Name,
                Food = x.Food.Select(y => new FoodGetDto { Id = y.Food.Id, Name = y.Food.Name, Cost = y.Food.Cost }).ToList()
            }).ToList();
            return menus;
        }

        // GET api/menu
        [HttpGet]
        public List<MenuGetDto> Get()
        {
            return GetMenus();
        }

        // GET api/menu/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            MenuGetDto menu = GetMenus().FirstOrDefault(x => x.Id == id);
            if (menu == null)
                return NotFound();
            return Ok(menu);
        }

        // POST api/menu
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MenuPostDto menu)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Menu newMenu = new Menu()
            {
                Name = menu.Name
            };

            await _context.Menus.AddAsync(newMenu);

            await _context.SaveChangesAsync();

            return Ok(menu);

        }

        // PUT api/menu/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MenuPutDto menu)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (menu.Id != id)
                return NotFound();

            var existingMenu = await _context.Menus.FindAsync(menu.Id);
            if (existingMenu == null || existingMenu.Id != menu.Id)
                return NotFound();

            existingMenu.Name = menu.Name;

            _context.Menus.Update(existingMenu);
            await _context.SaveChangesAsync();

            MenuGetDto newMenu = GetMenus().FirstOrDefault(x => x.Id == existingMenu.Id);
            if (newMenu == null)
                return NotFound();
            return Ok(newMenu);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existCheck = await _context.Menus.FindAsync(id);
            if (existCheck == null)
                return NotFound();

            _context.Menus.Remove(existCheck);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
