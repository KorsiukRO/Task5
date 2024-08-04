using System.Text.RegularExpressions;

namespace F1.Application.Models;

public partial class Race
{
    public required Guid Id { get; init; }

    public string Slug => GenerateSlug();
    public required string NameRace { get; init; }
    
    public required int PassabilityRace { get; init; }
    
    private string GenerateSlug()
    {
        var sluggedNameRace =
            SlugRegex().Replace(NameRace, string.Empty)
                .ToLower().Replace(" ", "-");

        return $"{sluggedNameRace}-grand-prix-{DateTime.Now.Year}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 10)]
    private static partial Regex SlugRegex();
}