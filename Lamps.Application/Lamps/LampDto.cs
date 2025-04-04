using System.ComponentModel.DataAnnotations;

namespace Lamps.Application.Lamps
{
    public class LampDto
    {
        [Required]
        public double r_top { get; set; }

        [Required]
        public double r_lamp { get; set; }

        [Required]
        public double r_middle { get; set; }

        [Required]
        public double r_base { get; set; }

        [Required]
        public double h_top { get; set; }

        [Required]
        public double h_lamp { get; set; }

        [Required]
        public double h_base_top { get; set; }

        [Required]
        public double h_base_bottom { get; set; }
    }
}
