using FluentValidation;
using Lamps.Domain;

namespace Lamps.Application.Lamps
{
    public class LampValidator : AbstractValidator<Lamp>
    {
        public LampValidator()
        {
            // Każdy parametr musi być liczbą dodatnią
            RuleFor(x => x.r_top).GreaterThan(0).WithMessage("r_top must be a positive number.");
            RuleFor(x => x.r_lamp).GreaterThan(0).WithMessage("r_lamp must be a positive number.");
            RuleFor(x => x.r_middle).GreaterThan(0).WithMessage("r_middle must be a positive number.");
            RuleFor(x => x.r_base).GreaterThan(0).WithMessage("r_base must be a positive number.");
            RuleFor(x => x.h_top).GreaterThan(0).WithMessage("h_top must be a positive number.");
            RuleFor(x => x.h_lamp).GreaterThan(0).WithMessage("h_lamp must be a positive number.");
            RuleFor(x => x.h_base_top).GreaterThan(0).WithMessage("h_base_top must be a positive number.");
            RuleFor(x => x.h_base_bottom).GreaterThan(0).WithMessage("h_base_bottom must be a positive number.");

            // r_base powinno być większe niż r_middle
            RuleFor(x => x.r_base)
                .GreaterThan(x => x.r_middle)
                .WithMessage("r_base must be greater than r_middle.");

            // r_top powinno być mniejsze niż r_lamp
            RuleFor(x => x.r_top)
                .LessThan(x => x.r_lamp)
                .WithMessage("r_top must be smaller than r_lamp.");

            // h_top powinno być krótsze niż h_lamp
            RuleFor(x => x.h_top)
                .LessThan(x => x.h_lamp)
                .WithMessage("h_top must be shorter than h_lamp.");

            // h_lamp powinno być większe lub równe sumie h_base_top i h_base_bottom
            RuleFor(x => x.h_lamp)
                .GreaterThanOrEqualTo(x => x.h_base_top + x.h_base_bottom)
                .WithMessage("h_lamp must be greater or equal to the sum of h_base_top and h_base_bottom.");
        }
    }
}
