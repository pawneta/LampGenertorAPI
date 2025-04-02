using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Cars.Domain;
using Cars.Application.Cars;
using System.Data;

namespace Cars.Application.Cars
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator(){
        RuleFor(x => x.Brand).NotEmpty().WithMessage("Brand is required");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required");
            RuleFor(x => x.DoorsNumber).InclusiveBetween(2, 10).NotEmpty().WithMessage("Door number is required and must be between 2 and 10");
            RuleFor(x => x.LuggageCapacity).NotEmpty().WithMessage("Luggage capacity is required");
            RuleFor(x => x.EngineCapacity).NotEmpty().WithMessage("EngineCapacity is required");
            RuleFor(x => x.FuelType).IsInEnum().WithMessage("FuelType is required");
            RuleFor(x => x.ProductionDate).NotEmpty().WithMessage("ProductionDate is required");
            RuleFor(x => x.CarFuelConsumption).GreaterThan(0).NotEmpty().WithMessage("CarFuelConsumption is required");
            RuleFor(x => x.FuelCapacity).IsInEnum().WithMessage("BodyType is required"); ;

    }

    }
}
