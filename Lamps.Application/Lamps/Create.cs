using System;
using Lamps.Infrastructure;
using MediatR;
using Lamps.Domain;
using FluentValidation;
using Lamps.Application.Lamps;
using Lamps.Infrastructure;
using System.Runtime.ConstrainedExecution;

namespace Lamps.Application.Lamps
{
    public class Create
    {

        public class Command : IRequest<Result<Unit>>
        {
            public required Lamp Lamp { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Lamp).SetValidator(new LampValidator());
            }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }


            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                //var newCar = new Car
                //{
                //    Brand = request.Car.Brand,
                //    Model = request.Car.Model,
                //    DoorsNumber = request.Car.DoorsNumber,
                //    LuggageCapacity = request.Car.LuggageCapacity,
                //    EngineCapacity = request.Car.EngineCapacity,
                //    FuelType = request.Car.FuelType,
                //    ProductionDate = request.Car.ProductionDate,
                //    CarFuelConsumption = request.Car.CarFuelConsumption,
                //    FuelCapacity = request.Car.FuelCapacity,
                //};


                //await _context.Cars.AddAsync(newCar, cancellationToken);
                _context.Lamps.Add(request.Lamp);
                var result = await _context.SaveChangesAsync() > 0;
                return Result<Unit>.Success(Unit.Value);
                //await _context.SaveChangesAsync(cancellationToken);
                //return Unit.Value;
            }

        }


    }
}