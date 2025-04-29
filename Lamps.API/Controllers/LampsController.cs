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

            var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var stlDir = Path.Combine(webRootPath, "stl_files");

            if (!Directory.Exists(stlDir))
            {
                Directory.CreateDirectory(stlDir);
            }

            var lampId = Guid.NewGuid().ToString();
            var lampDir = Path.Combine(stlDir, lampId);
            Directory.CreateDirectory(lampDir);

            try
            {
                bool success = await LampModelGenerator.GenerateAllParts(lampDto, lampDir);
                if (!success)
                {
                    return StatusCode(500, "Failed to generate STL files.");
                }

                var baseUrl = $"{Request.Scheme}://{Request.Host}/stl_files/{lampId}";

                return Ok(new
                {
                    baseUrl = baseUrl,
                    parts = new
                    {
                        basePart = $"{baseUrl}/base.stl",
                        middlePart = $"{baseUrl}/middle.stl",
                        lampBodyPart = $"{baseUrl}/lamp_body.stl",
                        topPart = $"{baseUrl}/top.stl"
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating STL files: {ex.Message}");
            }
        }




    }
}


