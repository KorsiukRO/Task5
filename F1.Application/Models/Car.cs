using System.Text.RegularExpressions;

namespace F1.Application.Models;

public partial class Car
{
    public required Guid Id { get; init; }

    public string Slug => GenerateSlug();

    public required int Speed { get; set; }


    public required int Passability { get; set; }
    public required string Manufacturer { get; init; } 
    
    public required string Model { get; init; }  
    
    private string GenerateSlug()
    {
        var sluggedManufacturer =
            SlugRegex().Replace(Manufacturer, string.Empty)
                .ToLower().Replace(" ", "-");
        var sluggedModel = 
            SlugRegex().Replace(Model, string.Empty)
                .ToLower().Replace(" ", "-");

            return $"{sluggedManufacturer}-{sluggedModel}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 10)]
    private static partial Regex SlugRegex();
}