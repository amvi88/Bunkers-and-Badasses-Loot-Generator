
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.Json;

using Business.Factories;
using Business.Models.Config;

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
builder.Services.AddTransient<IItemFactory<Business.Models.Grenade>, GrenadeFactory>();
builder.Services.AddTransient<IItemFactory<Business.Models.Shield>, ShieldFactory>();
builder.Services.AddTransient<IGunFactory, GunFactory>();
builder.Services.AddTransient<IPotionFactory,PotionFactory>();
builder.Services.AddTransient<IRelicFactory,RelicFactory>();

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