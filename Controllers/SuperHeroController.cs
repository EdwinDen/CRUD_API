using CRUD_API.Data;
using CRUD_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRUD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {

        private readonly DataContext _context;

        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllHeroes()
        {
            var heroes = await _context.SuperHeroes.ToListAsync();

            return Ok(heroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetSingleHero(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);

            if (hero == null)
                return NotFound("No Hero Found By the id "+id);

            return Ok(hero);
        }

        [HttpPut]
        public async Task<ActionResult<SuperHero>> UpdateHero(SuperHero updatedHero)
        {
            var dbhero = await _context.SuperHeroes.FindAsync(updatedHero.Id);

            if (dbhero == null)
                return NotFound("No Hero Found");

            dbhero.Name = updatedHero.Name;
            dbhero.FirstName = updatedHero.FirstName;
            dbhero.LastName = updatedHero.LastName;
            dbhero.Place = updatedHero.Place;

            await _context.SaveChangesAsync();

            return Ok(updatedHero);
        }


        [HttpDelete]
        public async Task<ActionResult<SuperHero>> DeleteHero(int id)
        {
            var dbhero = await _context.SuperHeroes.FindAsync(id);

            if (dbhero == null)
                return NotFound("No Hero Found");

            _context.SuperHeroes.Remove(dbhero);            
            await _context.SaveChangesAsync();

            return Ok(dbhero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero([FromBody] SuperHero hero)
        {
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }
    }
}
