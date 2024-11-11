using System.Text;
using F1.Application;
using F1.Application.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Task5.Mapping;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"]!)), 
        ValidateIssuerSigningKey = true, 
        ValidateLifetime = true,         
        ValidIssuer = config["Jwt:Issuer"], 
        ValidAudience = config["Jwt:Audience"], 
        ValidateIssuer = true,           
        ValidateAudience = true          
    };
});

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("FanOrAllInclusiveUsers", policy => 
        policy.RequireClaim("SubscriptionType", "fan", "all-inclusive"));
    
    x.AddPolicy("AllInclusiveUser", policy => 
        policy.RequireClaim("SubscriptionType", "all-inclusive"));
    
    x.AddPolicy("VipOrAllInclusiveUsers", policy => 
        policy.RequireClaim("SubscriptionType", "vip", "all-inclusive"));
    x.AddPolicy("FanOrVIPOrAllInclusiveUsers", policy =>
        policy.RequireClaim("SubscriptionType", "fan", "vip", "all-inclusive"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();