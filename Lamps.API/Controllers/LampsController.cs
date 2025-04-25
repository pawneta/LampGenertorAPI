using Lamps.Application.Lamps;
using Lamps.Domain;
using Lamps.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Collections.Generic;


namespace Lamps.API.Controllers
{
    public class LampsController : BaseApiController
    {
        


        private readonly DataContext _context;
        private readonly IMediator _mediator;
        public LampsController(DataContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        //[HttpGet]
        //public async Task<ActionResult<List<Lamp>>> GetLamps()
        //{
        //    return await _context.Lamps.ToListAsync();
        //}



        [HttpPost]
        public async Task<IActionResult> CreateLamp([FromBody] LampDto lampDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the full path to wwwroot
            var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Create stl_files directory if it doesn't exist
            var stlDir = Path.Combine(webRootPath, "stl_files");
            if (!Directory.Exists(stlDir))
            {
                Directory.CreateDirectory(stlDir);
            }

            var fileName = $"lamp_{Guid.NewGuid()}.stl";
            //var filePath = Path.Combine(stlDir, fileName);
            var filePath = Path.Combine("wwwroot", "stl_files", fileName);

            try
            {
                bool success = await LampModelGenerator.GenerateSTL(lampDto, filePath);
                if (!success)
                {
                    return StatusCode(500, "Failed to generate STL file.");
                }

                var fileUrl = $"{Request.Scheme}://{Request.Host}/stl_files/{fileName}";
                return Ok(new { fileUrl });
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                return StatusCode(500, $"Error generating STL file: {ex.Message}");
            }
        }



    }
}


