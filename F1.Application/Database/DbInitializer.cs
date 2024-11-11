using Dapper;

namespace F1.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
                                          create table if not exists cars (
                                          id UUID primary key,
                                          slug TEXT not null,
                                          speed integer not null,
                                          passability integer not null,
                                          manufacturer Text not null,
                                          model Text not null);
                                      """);

        await connection.ExecuteAsync("""
                                          create unique index concurrently if not exists cars_slug_idx
                                          on cars
                                          using btree(slug);
                                      """);

        await connection.ExecuteAsync("""
                                          create table if not exists races (
                                          id UUID primary key,
                                          slug TEXT not null,
                                          NameRace Text not null,
                                          PassabilityRace integer not null,
                                          Location Text not null,
                                          DateEvent DATE not null,
                                          BasicPrice integer not null,
                                          SubscriptionType Text not null);
                                      """);

        await connection.ExecuteAsync("""
                                          create unique index concurrently if not exists races_slug_idx
                                          on races
                                          using btree(slug);
                                      """);

        await connection.ExecuteAsync("""
                                          create table if not exists users (
                                          id UUID primary key,
                                          fullname TEXT not null,
                                          email TEXT not null,
                                          password TEXT not null,
                                          subscription_type TEXT not null);
                                      """);

        await connection.ExecuteAsync("""
                                          create unique index concurrently if not exists users_email_idx
                                          on users
                                          using btree(email);
                                      """);

        await connection.ExecuteAsync("""
                                          create table if not exists tickets (
                                          ticket_id UUID PRIMARY KEY,
                                          user_id UUID NOT NULL,
                                          race_id UUID NOT NULL,
                                          date_event DATE NOT NULL,
                                          price int NOT NULL,
                                          ticket_type VARCHAR(50),
                                      
                                          FOREIGN KEY (user_id) REFERENCES users(id),
                                          FOREIGN KEY (race_id) REFERENCES races(id)
                                      );
                                      """);

        if (!(await connection.QueryAsync<Guid>("SELECT id FROM cars")).Any())
        {
            await connection.ExecuteAsync("""
                                              INSERT INTO cars (id, slug, speed, passability, manufacturer, model) VALUES
                                              (gen_random_uuid(), 'alpine-renault-a524', 330, 25, 'Alpine-Renault', 'A524'),
                                              (gen_random_uuid(), 'aston-martin-amr24', 340, 28, 'Aston Martin', 'AMR24'),
                                              (gen_random_uuid(), 'ferrari-sf-24', 350, 30, 'Ferrari', 'SF-24'),
                                              (gen_random_uuid(), 'haas-ferrari-vf-24', 320, 24, 'Haas-Ferrari', 'VF-24'),
                                              (gen_random_uuid(), 'sauber-ferrari-c44', 315, 26, 'Sauber-Ferrari', 'C44'),
                                              (gen_random_uuid(), 'mclaren-mercedes-mcl38', 325, 27, 'McLaren-Mercedes', 'MCL38'),
                                              (gen_random_uuid(), 'mercedes-f1-w15', 340, 28, 'Mercedes', 'F1 W15'),
                                              (gen_random_uuid(), 'rb-honda-vcarb-01', 335, 29, 'RB-Honda', 'VCARB 01'),
                                              (gen_random_uuid(), 'red-bull-racing-rb20', 345, 30, 'Red Bull Racing', 'RB20'),
                                              (gen_random_uuid(), 'williams-mercedes-fw46', 310, 23, 'Williams-Mercedes', 'FW46');
                                          """);
        }

        if (!(await connection.QueryAsync<Guid>("SELECT id FROM races")).Any())
        {
            await connection.ExecuteAsync("""
                                              INSERT INTO races (id, slug, NameRace, PassabilityRace, Location, DateEvent, BasicPrice, SubscriptionType) VALUES
                                              ('564e3535-28da-46a7-9fe1-104357490687', 'qatar-grand-prix-2024', 'Qatar Grand Prix', 40, 'Losail International Circuit', '2024-12-08', 140, 'vip'),
                                              (gen_random_uuid(), 'bahrain-grand-prix-2024', 'Bahrain Grand Prix', 50, 'Bahrain International Circuit, Sakhir', '2024-03-02', 120, 'fan'),
                                              (gen_random_uuid(), 'saudi-arabian-grand-prix-2024', 'Saudi Arabian Grand Prix', 55, 'Jeddah Street Circuit, Jeddah', '2024-03-09', 130, 'fan'),
                                              (gen_random_uuid(), 'australian-grand-prix-2024', 'Australian Grand Prix', 60, 'Albert Park, Melbourne', '2024-03-24', 135, 'vip'),
                                              (gen_random_uuid(), 'japanese-grand-prix-2024', 'Japanese Grand Prix', 70, 'Suzuka Circuit, Suzuka', '2024-04-07', 140, 'vip'),
                                              (gen_random_uuid(), 'chinese-grand-prix-2024', 'Chinese Grand Prix', 65, 'Shanghai International Circuit, Shanghai', '2024-04-21', 125, 'fan'),
                                              (gen_random_uuid(), 'miami-grand-prix-2024', 'Miami Grand Prix', 75, 'Miami International Autodrome, Miami Gardens, Florida', '2024-05-05', 150, 'all-inclusive'),
                                              (gen_random_uuid(), 'emilia-romagna-grand-prix-2024', 'Emilia-Romagna Grand Prix', 68, 'Autodromo Enzo e Dino Ferrari, Imola', '2024-05-19', 140, 'vip'),
                                              (gen_random_uuid(), 'monaco-grand-prix-2024', 'Monaco Grand Prix', 80, 'Monte Carlo Street Circuit, Monte Carlo', '2024-05-26', 160, 'vip'),
                                              (gen_random_uuid(), 'canadian-grand-prix-2024', 'Canadian Grand Prix', 72, 'Circuit Gilles Villeneuve, Montreal', '2024-06-09', 145, 'fan'),
                                              (gen_random_uuid(), 'spanish-grand-prix-2024', 'Spanish Grand Prix', 67, 'Circuit de Barcelona-Catalunya, Barcelona', '2024-06-23', 130, 'fan'),
                                              (gen_random_uuid(), 'austrian-grand-prix-2024', 'Austrian Grand Prix', 75, 'Red Bull Ring, Spielberg', '2024-06-30', 135, 'all-inclusive'),
                                              (gen_random_uuid(), 'british-grand-prix-2024', 'British Grand Prix', 78, 'Silverstone Circuit, Silverstone', '2024-07-07', 150, 'vip'),
                                              (gen_random_uuid(), 'hungarian-grand-prix-2024', 'Hungarian Grand Prix', 70, 'Hungaroring, Budapest', '2024-07-21', 120, 'fan'),
                                              (gen_random_uuid(), 'belgian-grand-prix-2024', 'Belgian Grand Prix', 77, 'Circuit de Spa-Francorchamps, Spa', '2024-07-28', 155, 'all-inclusive'),
                                              (gen_random_uuid(), 'dutch-grand-prix-2024', 'Dutch Grand Prix', 74, 'Circuit Zandvoort, Zandvoort', '2024-08-25', 140, 'vip'),
                                              (gen_random_uuid(), 'italian-grand-prix-2024', 'Italian Grand Prix', 80, 'Autodromo Nazionale Monza, Monza', '2024-09-01', 145, 'vip'),
                                              (gen_random_uuid(), 'azerbaijan-grand-prix-2024', 'Azerbaijan Grand Prix', 72, 'Baku City Circuit, Baku', '2024-09-15', 140, 'fan'),
                                              (gen_random_uuid(), 'singapore-grand-prix-2024', 'Singapore Grand Prix', 76, 'Marina Bay Street Circuit, Singapore', '2024-09-22', 155, 'all-inclusive'),
                                              (gen_random_uuid(), 'us-grand-prix-2024', 'United States Grand Prix', 75, 'Circuit of the Americas, Austin, Texas', '2024-10-20', 150, 'vip'),
                                              (gen_random_uuid(), 'mexican-grand-prix-2024', 'Mexican Grand Prix', 70, 'Autódromo Hermanos Rodríguez, Mexico City', '2024-10-27', 135, 'fan'),
                                              (gen_random_uuid(), 'brazilian-grand-prix-2024', 'Brazilian Grand Prix', 78, 'Interlagos, São Paulo', '2024-11-03', 145, 'all-inclusive'),
                                              (gen_random_uuid(), 'las-vegas-grand-prix-2024', 'Las Vegas Grand Prix', 85, 'Las Vegas Street Circuit, Paradise, Nevada', '2024-11-23', 160, 'vip'),
                                              (gen_random_uuid(), 'abu-dhabi-grand-prix-2024', 'Abu Dhabi Grand Prix', 80, 'Yas Marina Circuit, Abu Dhabi', '2024-12-08', 150, 'all-inclusive');
                                          """);
        }
    }
}