using Microsoft.AspNetCore.Mvc;
using RocksAPI.Models;
using RocksAPI.Data;
using System.Collections.Generic;
using System.Linq;

namespace RocksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectorsController : ControllerBase
    {
        private readonly RockStore _rockStore;

        public CollectorsController(RockStore rockStore)
        {
            _rockStore = rockStore;
        }

        [HttpGet]
        public IEnumerable<Collector> Get()
        {
            return _rockStore.RockData.Collectors ?? new List<Collector>();
        }

        [HttpGet("{id}")]
        public ActionResult<Collector> Get(string id)
        {
            if (_rockStore.RockData.Collectors == null) return NotFound();
            var collector = _rockStore.RockData.Collectors.FirstOrDefault(c => c.CollectorId == id);
            if (collector == null)
            {
                return NotFound();
            }
            return collector;
        }

        [HttpPost]
        public ActionResult<Collector> Post([FromBody] Collector collector)
        {
            if (_rockStore.RockData.Collectors == null) _rockStore.RockData.Collectors = new List<Collector>();

            collector.CollectorId = $"collector-id-{_rockStore.RockData.Collectors.Count + 1}";
            _rockStore.RockData.Collectors.Add(collector);
            return CreatedAtAction(nameof(Get), new { id = collector.CollectorId }, collector);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Collector updatedCollector)
        {
            if (_rockStore.RockData.Collectors == null) return NotFound();
            var collector = _rockStore.RockData.Collectors.FirstOrDefault(c => c.CollectorId == id);
            if (collector == null)
            {
                return NotFound();
            }

            collector.Name = updatedCollector.Name;
            collector.Email = updatedCollector.Email;
            collector.MemberSince = updatedCollector.MemberSince;
            collector.PreferredCollectionTypes = updatedCollector.PreferredCollectionTypes;
            collector.Notes = updatedCollector.Notes;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_rockStore.RockData.Collectors == null) return NotFound();
            var collector = _rockStore.RockData.Collectors.FirstOrDefault(c => c.CollectorId == id);
            if (collector == null)
            {
                return NotFound();
            }
            _rockStore.RockData.Collectors.Remove(collector);
            return NoContent();
        }
    }
}