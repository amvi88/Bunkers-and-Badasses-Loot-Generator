using Microsoft.AspNetCore.Http.Json;

using Business.Factories;
using Business.Models.Config;
using Business.Models.Builder;
using Business.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("weaponarchetypes.json", true);
builder.Configuration.AddJsonFile("weaponcustomization.json", true);
builder.Configuration.AddJsonFile("weaponprefixes.json", true);
builder.Configuration.AddJsonFile("weaponredtext.json", true);
builder.Configuration.AddJsonFile("guilds.json", true);
builder.Configuration.AddJsonFile("potions.json", true);
builder.Configuration.AddJsonFile("tinytinaspotions.json", true);
builder.Configuration.AddJsonFile("relics.json", true);

// Add services to the container.
builder.Services.Configure<JsonOptions>(options =>
{
    var enumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
    options.SerializerOptions.Converters.Add(enumConverter);
});
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.Configure<GuildConfigurationOptions>(builder.Configuration.GetSection("GuildConfiguration"));
builder.Services.Configure<WeaponCustomizationOptions>(builder.Configuration.GetSection("WeaponCustomization"));
builder.Services.Configure<WeaponArchetypesOptions>(builder.Configuration.GetSection("WeaponArchetypes"));
builder.Services.Configure<PotionConfigurationOptions>(builder.Configuration.GetSection("PotionConfiguration"));
builder.Services.Configure<RelicConfigurationOptions>(builder.Configuration.GetSection("RelicConfiguration"));
builder.Services.AddTransient<IItemFactory<Grenade, GrenadeFactoryParameters>, GrenadeFactory>();
builder.Services.AddTransient<IItemFactory<Shield, ShieldFactoryParameters>, ShieldFactory>();
builder.Services.AddTransient<IItemFactory<Gun, GunFactoryParameters>, GunFactory>();
builder.Services.AddTransient<IItemFactory<Potion, BaseFactoryParameters>,PotionFactory>();
builder.Services.AddTransient<IItemFactory<Relic, RelicFactoryParameters>,RelicFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();