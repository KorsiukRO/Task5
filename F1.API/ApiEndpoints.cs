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
        public const string GetAllSort = $"{Base}/sort";
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Races
    {
        private const string Base = $"{ApiBase}/races";

        public const string Create = Base;
        public const string Get = $"{Base}/{{idOrSlug}}";
        public const string GetByNameRace = $"{Base}/search";
        public const string GetAll = Base;
        public const string GetAllSortByDateEvent = $"{Base}/sort";
        public const string GetAllInRange = $"{Base}/range";
        public const string Update =$"{Base}/{{id:guid}}";
        public const string Delete =$"{Base}/{{id:guid}}";
        public const string Start = $"{ApiBase}/race";
    }

    public static class Users
    {
        private const string Base = $"{ApiBase}/users";
        
        public const string Register  = $"{ApiBase}/register";
        public const string Login = $"{ApiBase}/login";
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetByEmailUser = $"{Base}/search";
        public const string GetAll = Base;
        public const string Update =$"{Base}/{{id:guid}}";
        public const string Delete =$"{Base}/{{id:guid}}";
    }

    public static class Tickets
    {
        private const string Base = $"{ApiBase}/tickets";

        public const string Buy = $"{Base}/buy";
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetMyTickets = Base;
        public const string GetTicketsByRace = $"{Base}/search/by-race";
        public const string GetTicketsByDate = $"{Base}/search/by-date";
        public const string GetTopTickets = $"{Base}/top";
    }
}