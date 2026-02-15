
using Goal_Project.Models;
using Goal_Project.Repository.Implimentation;
using Goal_Project.Repository.Interface;
using Goal_Project.Service.Implimentation;
using Goal_Project.Service.Interface;
using Goal_Project.UnitOfWork;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
try
{
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {

            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    //builder.Services.AddOpenApi();
    builder.Services.Configure<MongoSettings>(
        builder.Configuration.GetSection("MongoSettings"));

    builder.Services.AddScoped<IVenueRepo, VenueRepo>();
    builder.Services.AddScoped<IVenueService, VenueService>();
    builder.Services.AddScoped<ITeamRepo, TeamRepo>();
    builder.Services.AddScoped<ITeamService, TeamService>();
    builder.Services.AddScoped<IPlayerRepo, PlayerRepo>();
    builder.Services.AddScoped<IPlayerService, PlayerService>();
    builder.Services.AddScoped<IMatchRepo, MatchRepo>();
    builder.Services.AddScoped<IMatchService, MatchService>();
    builder.Services.AddScoped<IGoalRepo, GoalRepo>();
    builder.Services.AddScoped<IGoalService, GoalService>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();




    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAngularApp");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

