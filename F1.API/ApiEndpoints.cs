namespace Task5;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    
    public static class Cars
    {
        private const string Base = $"{ApiBase}/cars";

        public const string Create = Base;
        public const string Get = $"{Base}/{{idOrSlug}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Races
    {
        private const string Base = $"{ApiBase}/races";

        public const string Create = Base;
        public const string Get = $"{Base}/{{idOrSlug}}";
        public const string GetAll = Base;
        public const string Update =$"{Base}/{{id:guid}}";
        public const string Delete =$"{Base}/{{id:guid}}";
        public const string Start = $"{ApiBase}/race";
    }
}