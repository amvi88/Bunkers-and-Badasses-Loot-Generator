
using Business.Factories;
using Business.Models.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddControllers()
.AddJsonOptions(opts =>
    {
        var enumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<GuildConfigurationOptions>(builder.Configuration.GetSection("GuildConfiguration"));
builder.Services.Configure<WeaponCustomizationOptions>(builder.Configuration.GetSection("WeaponCustomization"));
builder.Services.Configure<WeaponArchetypesOptions>(builder.Configuration.GetSection("WeaponArchetypes"));
builder.Services.AddTransient<IItemFactory<Business.Models.Grenade>, GrenadeFactory>();
builder.Services.AddTransient<IItemFactory<Business.Models.Shield>, ShieldFactory>();
builder.Services.AddTransient<IGunFactory, GunFactory>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
