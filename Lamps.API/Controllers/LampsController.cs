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
        public IActionResult CreateLamp(Lamp lamp)
        {
            return Ok(new { message = "API działa, ale logika nie została jeszcze zaimplementowana." });
        }


    }
}


