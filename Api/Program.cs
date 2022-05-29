
using Business.Factories;
using Business.Models.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
.AddControllers()
.AddJsonOptions(opts =>
    {
        var enumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<GuildConfiguration>(builder.Configuration.GetSection("GuildConfiguration"));
builder.Services.Configure<WeaponCustomization>(builder.Configuration.GetSection("WeaponCustomization"));
builder.Services.AddTransient<IItemFactory<Business.Models.Grenade>, GrenadeFactory>();
builder.Services.AddTransient<IItemFactory<Business.Models.Shield>, ShieldFactory>();
builder.Services.AddTransient<IGunFactory, GunFactory>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
