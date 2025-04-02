using Cars.Application.Cars;
using Cars.Domain;
using Cars.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Cars.API.Controllers
{
    public class CarsController : BaseApiController
    {
        

        [HttpGet]
        public async Task<ActionResult<List<Car>>> GetCars()
        {
            var result = await Mediator.Send(new List.Query());
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);
            if (result.IsSuccess && result.Value == null) return NotFound();
            return BadRequest(result.Error);
            //return await Mediator.Send(new List.Query());
        }

        private readonly DataContext _context;

        public CarsController(DataContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public async Task<ActionResult<List<Car>>> GetCars()
        //{
        //    return await _context.Cars.ToListAsync();
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCar(Guid id)
        {
            var result = await Mediator.Send(new Details.Query { Id=id });
            if(result==null) return NotFound();
            if(result.IsSuccess && result.Value != null) return Ok(result.Value);
            if(result.IsSuccess && result.Value == null) return NotFound();
            return BadRequest(result.Error);
            //return await Mediator.Send(new Details.Query { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCar(Guid id, Car car)
        {
            car.Id = id;
            var result = await Mediator.Send(new Edit.Command { Car = car});
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);
            if (result.IsSuccess && result.Value == null) return NotFound();
            return BadRequest(result.Error);
            //await Mediator.Send(new Edit.Command { Car = car });
            //return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCar(Car car)
        {
            var result = await Mediator.Send(new Create.Command { Car = car });
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);
            if (result.IsSuccess && result.Value == null) return NotFound();
            return BadRequest(result.Error);
            // Send the command to create a new car
            //await Mediator.Send(new Create.Command { Car = car });
            //return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command { Id = id });
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);
            if (result.IsSuccess && result.Value == null) return NotFound();
            return BadRequest(result.Error);
            //await Mediator.Send(new Delete.Command { Id = id });
            //return NoContent();
        }
    }
}


