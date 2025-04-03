using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lamps.Domain;

namespace Lamps.Infrastructure
{
     public class Seed
    {
       public static async Task SeedData(DataContext context)
        {
            if (context.Lamps.Any()) return;
            var lamps = new List<Lamp>
            {
                new Lamp
                {


                },
                 new Lamp
                {


                }
            };
            await context.Lamps.AddRangeAsync(lamps);

            await context.SaveChangesAsync();
        }

    }
}

