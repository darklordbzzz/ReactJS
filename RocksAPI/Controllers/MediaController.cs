using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RocksAPI.Data;
using RocksAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RocksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly RockStore _rockStore;
        private readonly string _uploadPath;

        public MediaController(RockStore rockStore)
        {
            _rockStore = rockStore;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        [HttpPost("upload/{sampleId}")]
        public async Task<IActionResult> Upload(string sampleId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Upload a file.");

            if (_rockStore.RockData.Samples == null)
            {
                return NotFound("Sample data not available.");
            }
            var sample = _rockStore.RockData.Samples.FirstOrDefault(s => s.SampleId == sampleId);
            if (sample == null)
            {
                return NotFound("Sample not found.");
            }

            var mediaId = Guid.NewGuid().ToString();
            var fileName = $"{mediaId}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var media = new Media
            {
                Id = mediaId,
                SampleId = sampleId,
                FileName = fileName,
                ContentType = file.ContentType,
                Url = $"/uploads/{fileName}"
            };

            // In a real app, we would have a separate list for media.
            // For now, we'll just return the media object.
            // You could extend the RockStore to hold a List<Media>

            return Ok(media);
        }
    }
}