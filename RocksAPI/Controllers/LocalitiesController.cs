using Microsoft.AspNetCore.Mvc;
using RocksAPI.Models;
using RocksAPI.Data;
using System.Collections.Generic;
using System.Linq;

namespace RocksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalitiesController : ControllerBase
    {
        private readonly RockStore _rockStore;

        public LocalitiesController(RockStore rockStore)
        {
            _rockStore = rockStore;
        }

        [HttpGet]
        public IEnumerable<Locality> Get()
        {
            return _rockStore.RockData.Localities ?? new List<Locality>();
        }

        [HttpGet("{id}")]
        public ActionResult<Locality> Get(string id)
        {
            if (_rockStore.RockData.Localities == null) return NotFound();
            var locality = _rockStore.RockData.Localities.FirstOrDefault(l => l.LocalityId == id);
            if (locality == null)
            {
                return NotFound();
            }
            return locality;
        }

        [HttpPost]
        public ActionResult<Locality> Post([FromBody] Locality locality)
        {
            if (_rockStore.RockData.Localities == null) _rockStore.RockData.Localities = new List<Locality>();

            locality.LocalityId = $"mindat-locality-id-{_rockStore.RockData.Localities.Count + 1}";
            _rockStore.RockData.Localities.Add(locality);
            return CreatedAtAction(nameof(Get), new { id = locality.LocalityId }, locality);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Locality updatedLocality)
        {
            if (_rockStore.RockData.Localities == null) return NotFound();
            var locality = _rockStore.RockData.Localities.FirstOrDefault(l => l.LocalityId == id);
            if (locality == null)
            {
                return NotFound();
            }

            locality.Name = updatedLocality.Name;
            locality.Country = updatedLocality.Country;
            locality.StateProvince = updatedLocality.StateProvince;
            locality.Region = updatedLocality.Region;
            locality.Latitude = updatedLocality.Latitude;
            locality.Longitude = updatedLocality.Longitude;
            locality.MindatUrl = updatedLocality.MindatUrl;
            locality.Notes = updatedLocality.Notes;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_rockStore.RockData.Localities == null) return NotFound();
            var locality = _rockStore.RockData.Localities.FirstOrDefault(l => l.LocalityId == id);
            if (locality == null)
            {
                return NotFound();
            }
            _rockStore.RockData.Localities.Remove(locality);
            return NoContent();
        }
    }
}