using Microsoft.AspNetCore.Mvc;
using RocksAPI.Models;
using RocksAPI.Data;
using System.Collections.Generic;
using System.Linq;

namespace RocksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RocksController : ControllerBase
    {
        private readonly RockStore _rockStore;

        public RocksController(RockStore rockStore)
        {
            _rockStore = rockStore;
        }

        [HttpGet]
        public IEnumerable<Rock> Get()
        {
            return _rockStore.RockData.Rocks ?? new List<Rock>();
        }

        [HttpGet("{id}")]
        public ActionResult<Rock> Get(string id)
        {
            if (_rockStore.RockData.Rocks == null) return NotFound();
            var rock = _rockStore.RockData.Rocks.FirstOrDefault(r => r.Id == id);
            if (rock == null)
            {
                return NotFound();
            }
            return rock;
        }

        [HttpPost]
        public ActionResult<Rock> Post([FromBody] Rock rock)
        {
            if (_rockStore.RockData.Rocks == null) _rockStore.RockData.Rocks = new List<Rock>();

            rock.Id = $"rock-id-{_rockStore.RockData.Rocks.Count + 1}";
            _rockStore.RockData.Rocks.Add(rock);
            return CreatedAtAction(nameof(Get), new { id = rock.Id }, rock);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Rock updatedRock)
        {
            if (_rockStore.RockData.Rocks == null) return NotFound();
            var rock = _rockStore.RockData.Rocks.FirstOrDefault(r => r.Id == id);
            if (rock == null)
            {
                return NotFound();
            }

            rock.Name = updatedRock.Name ?? rock.Name;
            rock.Type = updatedRock.Type ?? rock.Type;
            rock.SubType = updatedRock.SubType ?? rock.SubType;
            rock.Texture = updatedRock.Texture ?? rock.Texture;
            rock.MineralComposition = updatedRock.MineralComposition ?? rock.MineralComposition;
            rock.Origin = updatedRock.Origin ?? rock.Origin;
            rock.Notes = updatedRock.Notes ?? rock.Notes;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_rockStore.RockData.Rocks == null) return NotFound();
            var rock = _rockStore.RockData.Rocks.FirstOrDefault(r => r.Id == id);
            if (rock == null)
            {
                return NotFound();
            }
            _rockStore.RockData.Rocks.Remove(rock);
            return NoContent();
        }
    }
}