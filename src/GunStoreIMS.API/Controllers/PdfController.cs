using AutoMapper;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GunStoreIMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        public PdfController()
        {
            // any DI goes here
        }

        // GET api/pdf
        [HttpGet]
        public IActionResult Get() => Ok("Pdf endpoint is live");

        // POST api/pdf/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // save to temp, parse, delete…
            var tempPath = Path.GetTempFileName();
            await using var fs = System.IO.File.Create(tempPath);
            await file.CopyToAsync(fs);

            // TODO: parse PDF here

            System.IO.File.Delete(tempPath);
            return Ok(new { /* your metadata or parse results */ });
        }
    }
}
