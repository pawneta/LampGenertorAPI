using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Lamps.Application.Lamps;

public class LampModelGenerator
{
    private const string OpenScadPath = @"C:\Program Files\OpenSCAD\openscad.exe"; // Sprawdź swoją ścieżkę!

    public static async Task<bool> GenerateSTL(LampDto lamp, string outputPath)
    {
        try
        {
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string scadFilePath = Path.ChangeExtension(outputPath, ".scad");
            string scadCode = GenerateOpenSCADCode(lamp);

            await File.WriteAllTextAsync(scadFilePath, scadCode);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = OpenScadPath,
                Arguments = $"-o \"{outputPath}\" \"{scadFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                await process.WaitForExitAsync();
                return process.ExitCode == 0;
            }
        }
        catch (Exception ex)
        {
            // Log the error if needed
            Console.WriteLine($"Error in GenerateSTL: {ex}");
            return false;
        }
    }

    private static string GenerateOpenSCADCode(LampDto lamp)
    {
        return $@"
module lamp(r_top, r_lamp, r_middle, r_base, h_top, h_lamp, h_base_top, h_base_bottom) {{
    
    // Poprawiona formuła r_upper_lamp
    r_upper_lamp = r_top+(((r_lamp - r_top)*h_top) / (h_lamp + h_top));
    
    // Podstawa
    cylinder(h_base_bottom, r1=r_base, r2=r_middle);

    // Górna część podstawy
    translate([0,0,h_base_bottom])
    cylinder(h_base_top, r1=r_middle, r2=r_lamp);

    // Główna część lampy
    translate([0,0,h_base_bottom + h_base_top])
    cylinder(h_lamp, r1=r_lamp, r2=r_upper_lamp);

    // Górna część lampy
    translate([0,0,h_base_bottom + h_base_top + h_lamp])
    cylinder(h_top, r1=r_upper_lamp, r2=r_top);
}}

lamp(
    r_top = {lamp.r_top},
    r_lamp = {lamp.r_lamp},
    r_middle = {lamp.r_middle},
    r_base = {lamp.r_base},
    h_top = {lamp.h_top},
    h_lamp = {lamp.h_lamp},
    h_base_top = {lamp.h_base_top},
    h_base_bottom = {lamp.h_base_bottom}
);";
    }
}
