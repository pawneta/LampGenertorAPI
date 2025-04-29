using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using Lamps.Application.Lamps;
using Lamps.Domain;
//using static System.Net.WebRequestMethods;

public class LampModelGenerator
{
    private const string OpenScadPath = @"C:\Program Files\OpenSCAD\openscad.exe"; // Upewnij się, że ścieżka jest poprawna!

    /// <summary>
    /// Generuje pojedynczy plik STL dla całej lampy (wszystkie części razem).
    /// </summary>
    public static async Task<bool> GenerateSTL(LampDto lamp, string outputPath)
    {
        try
        {
            var directory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string scadFilePath = Path.ChangeExtension(outputPath, ".scad");
            string scadCode = GenerateFullLampOpenSCAD(lamp);

            await File.WriteAllTextAsync(scadFilePath, scadCode);

            var startInfo = new ProcessStartInfo
            {
                FileName = OpenScadPath,
                Arguments = $"-o \"{outputPath}\" \"{scadFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();
                await process.WaitForExitAsync();
                return process.ExitCode == 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GenerateSTL: {ex}");
            return false;
        }
    }

    /// <summary>
    /// Generuje osobne pliki STL dla każdej części lampy (osobne modele do kolorowania).
    /// </summary>
    public static async Task<bool> GenerateAllParts(LampDto lamp, string outputDirectory)
    {
        try
        {
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            var parts = new (string Name, string Code)[]
            {
                ("base", GenerateBaseOpenSCAD(lamp)),
                ("middle", GenerateMiddleOpenSCAD(lamp)),
                ("top", GenerateTopOpenSCAD(lamp))
            };

            foreach (var (name, scadCode) in parts)
            {
                string scadPath = Path.Combine(outputDirectory, $"{name}.scad");
                string stlPath = Path.Combine(outputDirectory, $"{name}.stl");

                await File.WriteAllTextAsync(scadPath, scadCode);

                var startInfo = new ProcessStartInfo
                {
                    FileName = OpenScadPath,
                    Arguments = $"-o \"{stlPath}\" \"{scadPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    await process.WaitForExitAsync();
                    if (process.ExitCode != 0)
                        return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GenerateAllParts: {ex}");
            return false;
        }
    }

    // --- Kod generujący SCAD dla jednej lampy (stare GenerateOpenSCADCode) ---

    private static string GenerateFullLampOpenSCAD(LampDto lamp)
    {
        double r_upper_lamp = lamp.r_top + (((lamp.r_lamp - lamp.r_top) * lamp.h_top) / (lamp.h_lamp + lamp.h_top));
        return $@"
r_upper_lamp = {lamp.r_top.ToString("0.##", CultureInfo.InvariantCulture)} + ((({lamp.r_lamp.ToString("0.##", CultureInfo.InvariantCulture)} - {lamp.r_top.ToString("0.##", CultureInfo.InvariantCulture)}) * {lamp.h_top.ToString("0.##", CultureInfo.InvariantCulture)}) / ({lamp.h_lamp.ToString("0.##", CultureInfo.InvariantCulture)} + {lamp.h_top.ToString("0.##", CultureInfo.InvariantCulture)}));

cylinder(h={lamp.h_base_bottom.ToString("0.##", CultureInfo.InvariantCulture)}, r1={lamp.r_base.ToString("0.##", CultureInfo.InvariantCulture)}, r2={lamp.r_middle.ToString("0.##", CultureInfo.InvariantCulture)});

translate([0,0,{lamp.h_base_bottom.ToString("0.##", CultureInfo.InvariantCulture)}])
cylinder(h={lamp.h_base_top.ToString("0.##", CultureInfo.InvariantCulture)}, r1={lamp.r_middle.ToString("0.##", CultureInfo.InvariantCulture)}, r2={lamp.r_lamp.ToString("0.##", CultureInfo.InvariantCulture)});

translate([0,0,{lamp.h_base_bottom.ToString("0.##", CultureInfo.InvariantCulture) + lamp.h_base_top.ToString("0.##", CultureInfo.InvariantCulture)}])
color([1, 0.4, 0.7])
cylinder(h={lamp.h_lamp.ToString("0.##", CultureInfo.InvariantCulture)}, r1={lamp.r_lamp.ToString("0.##", CultureInfo.InvariantCulture)}, r2=r_upper_lamp);

translate([0,0,{lamp.h_base_bottom + lamp.h_base_top + lamp.h_lamp}])
cylinder(h={lamp.h_top.ToString("0.##", CultureInfo.InvariantCulture)}, r1={r_upper_lamp.ToString("0.##", CultureInfo.InvariantCulture)}, r2={lamp.r_top.ToString("0.##", CultureInfo.InvariantCulture)});
";
    }

    // --- Kody SCAD dla poszczególnych części lampy ---

    private static string GenerateBaseOpenSCAD(LampDto lamp)
    {
        
        return $@"
cylinder(h={lamp.h_base_bottom.ToString("0.##", CultureInfo.InvariantCulture)}, r1={lamp.r_base.ToString("0.##", CultureInfo.InvariantCulture)}, r2={lamp.r_middle.ToString("0.##", CultureInfo.InvariantCulture)});

translate([0,0,{lamp.h_base_bottom.ToString("0.##", CultureInfo.InvariantCulture)}])
cylinder(h={lamp.h_base_top.ToString("0.##", CultureInfo.InvariantCulture)}, r1={lamp.r_middle.ToString("0.##", CultureInfo.InvariantCulture)}, r2={lamp.r_lamp.ToString("0.##", CultureInfo.InvariantCulture)});
";
    }

    private static string GenerateMiddleOpenSCAD(LampDto lamp)
    {
        double r_upper_lamp = lamp.r_top + (((lamp.r_lamp - lamp.r_top) * lamp.h_top) / (lamp.h_lamp + lamp.h_top));

        return $@"
translate([0,0,{(lamp.h_base_bottom + lamp.h_base_top).ToString("0.##", CultureInfo.InvariantCulture)}])
color([1, 0.4, 0.7])
cylinder(h={lamp.h_lamp.ToString("0.##", CultureInfo.InvariantCulture)}, r1={lamp.r_lamp.ToString("0.##", CultureInfo.InvariantCulture)}, r2={r_upper_lamp.ToString("0.##", CultureInfo.InvariantCulture)});
";
    }

    private static string GenerateTopOpenSCAD(LampDto lamp)
    {
        double r_upper_lamp = lamp.r_top + (((lamp.r_lamp - lamp.r_top) * lamp.h_top) / (lamp.h_lamp + lamp.h_top));

        return $@"
translate([0,0,{(lamp.h_base_bottom + lamp.h_base_top + lamp.h_lamp).ToString("0.##", CultureInfo.InvariantCulture)}])
cylinder(h={lamp.h_top.ToString("0.##", CultureInfo.InvariantCulture)}, r1={r_upper_lamp.ToString("0.##", CultureInfo.InvariantCulture)}, r2={lamp.r_top.ToString("0.##", CultureInfo.InvariantCulture)});
";
    }

}
