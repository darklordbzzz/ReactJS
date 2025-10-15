using Microsoft.AspNetCore.Mvc;
using RocksAPI.Models;
using RocksAPI.Data;
using System.Collections.Generic;
using System.Linq;

namespace RocksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MineralsController : ControllerBase
    {
        private readonly RockStore _rockStore;

        public MineralsController(RockStore rockStore)
        {
            _rockStore = rockStore;
        }

        [HttpGet]
        public IEnumerable<Mineral> Get()
        {
            return _rockStore.RockData.Minerals ?? new List<Mineral>();
        }

        [HttpGet("{id}")]
        public ActionResult<Mineral> Get(string id)
        {
            if (_rockStore.RockData.Minerals == null) return NotFound();
            var mineral = _rockStore.RockData.Minerals.FirstOrDefault(m => m.Id == id);
            if (mineral == null)
            {
                return NotFound();
            }
            return mineral;
        }

        [HttpPost]
        public ActionResult<Mineral> Post([FromBody] Mineral mineral)
        {
            if (_rockStore.RockData.Minerals == null) _rockStore.RockData.Minerals = new List<Mineral>();

            mineral.Id = $"mindat-mineral-id-{_rockStore.RockData.Minerals.Count + 1}";
            _rockStore.RockData.Minerals.Add(mineral);
            return CreatedAtAction(nameof(Get), new { id = mineral.Id }, mineral);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Mineral updatedMineral)
        {
            if (_rockStore.RockData.Minerals == null) return NotFound();
            var mineral = _rockStore.RockData.Minerals.FirstOrDefault(m => m.Id == id);
            if (mineral == null)
            {
                return NotFound();
            }

            mineral.Name = updatedMineral.Name;
            mineral.Formula = updatedMineral.Formula;
            mineral.Variety = updatedMineral.Variety;
            mineral.Crystallography = updatedMineral.Crystallography;
            mineral.PhysicalProperties = updatedMineral.PhysicalProperties;
            mineral.OpticalProperties = updatedMineral.OpticalProperties;
            mineral.OriginOccurrence = updatedMineral.OriginOccurrence;
            mineral.MindatUrl = updatedMineral.MindatUrl;
            mineral.Notes = updatedMineral.Notes;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_rockStore.RockData.Minerals == null) return NotFound();
            var mineral = _rockStore.RockData.Minerals.FirstOrDefault(m => m.Id == id);
            if (mineral == null)
            {
                return NotFound();
            }
            _rockStore.RockData.Minerals.Remove(mineral);
            return NoContent();
        }
    }
}