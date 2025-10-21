using Microsoft.AspNetCore.Mvc;
using RocksAPI.Models;
using RocksAPI.Data;
using System.Collections.Generic;
using System.Linq;

namespace RocksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SamplesController : ControllerBase
    {
        private readonly RockStore _rockStore;

        public SamplesController(RockStore rockStore)
        {
            _rockStore = rockStore;
        }

        [HttpGet]
        public IEnumerable<Sample> Get()
        {
            return _rockStore.RockData.Samples ?? new List<Sample>();
        }

        [HttpGet("{id}")]
        public ActionResult<Sample> Get(string id)
        {
            if (_rockStore.RockData.Samples == null) return NotFound();
            var sample = _rockStore.RockData.Samples.FirstOrDefault(s => s.SampleId == id);
            if (sample == null)
            {
                return NotFound();
            }
            return sample;
        }

        [HttpPost]
        public ActionResult<Sample> Post([FromBody] Sample sample)
        {
            if (_rockStore.RockData.Samples == null) _rockStore.RockData.Samples = new List<Sample>();

            sample.SampleId = $"collector-sample-{_rockStore.RockData.Samples.Count + 1}";
            _rockStore.RockData.Samples.Add(sample);
            return CreatedAtAction(nameof(Get), new { id = sample.SampleId }, sample);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Sample updatedSample)
        {
            if (_rockStore.RockData.Samples == null) return NotFound();
            var sample = _rockStore.RockData.Samples.FirstOrDefault(s => s.SampleId == id);
            if (sample == null)
            {
                return NotFound();
            }

            sample.CollectorId = updatedSample.CollectorId ?? sample.CollectorId;
            sample.DateAcquired = updatedSample.DateAcquired ?? sample.DateAcquired;
            sample.ItemType = updatedSample.ItemType ?? sample.ItemType;
            sample.MineralId = updatedSample.MineralId ?? sample.MineralId;
            sample.RockId = updatedSample.RockId ?? sample.RockId;
            sample.LocalityId = updatedSample.LocalityId ?? sample.LocalityId;
            sample.Description = updatedSample.Description ?? sample.Description;
            sample.WeightGrams = updatedSample.WeightGrams != 0 ? updatedSample.WeightGrams : sample.WeightGrams;
            sample.DimensionsCm = updatedSample.DimensionsCm ?? sample.DimensionsCm;
            sample.PricePaid = updatedSample.PricePaid ?? sample.PricePaid;
            sample.Source = updatedSample.Source ?? sample.Source;
            sample.Photos = updatedSample.Photos ?? sample.Photos;
            sample.Tags = updatedSample.Tags ?? sample.Tags;
            sample.Notes = updatedSample.Notes ?? sample.Notes;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_rockStore.RockData.Samples == null) return NotFound();
            var sample = _rockStore.RockData.Samples.FirstOrDefault(s => s.SampleId == id);
            if (sample == null)
            {
                return NotFound();
            }
            _rockStore.RockData.Samples.Remove(sample);
            return NoContent();
        }
    }
}