using Business.Factories;
using Models.Config;
using Models.Builder;
using Models;
using Business.Services;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http.Json;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;
using System.Text.Json.Serialization;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "B&B Loot Generator API",
                    Description = "An API for generating loot for B&B TTRPG",
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });
            });
            builder.Configuration.AddJsonFile("Configuration/weaponarchetypes.json", true);
            builder.Configuration.AddJsonFile("Configuration/weaponcustomization.json", true);
            builder.Configuration.AddJsonFile("Configuration/weaponprefixes.json", true);
            builder.Configuration.AddJsonFile("Configuration/weaponredtext.json", true);
            builder.Configuration.AddJsonFile("Configuration/weapongallery.json", true);
            builder.Configuration.AddJsonFile("Configuration/shieldgallery.json", true);
            builder.Configuration.AddJsonFile("Configuration/guilds.json", true);
            builder.Configuration.AddJsonFile("Configuration/potions.json", true);
            builder.Configuration.AddJsonFile("Configuration/tinytinaspotions.json", true);
            builder.Configuration.AddJsonFile("Configuration/relics.json", true);
            builder.Configuration.AddJsonFile("Configuration/moxxtails.json", true);
            builder.Configuration.AddJsonFile("Configuration/traumas.json", true);
            builder.Configuration.AddJsonFile("Configuration/dicechests.json", true);
            builder.Configuration.AddJsonFile("Configuration/unassumingchests.json", true);
            builder.Configuration.AddJsonFile("Configuration/cacherolls.json", true);
            builder.Configuration.AddJsonFile("Configuration/enemydrops.json", true);

            // Add services to the container.
            builder.Services.Configure<JsonOptions>(options =>
            {
                var enumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
                options.SerializerOptions.Converters.Add(enumConverter);
            });
            builder.Services.Configure<MvcJsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            builder.Services.Configure<GuildConfigurationOptions>(builder.Configuration.GetSection("GuildConfiguration"));
            builder.Services.Configure<WeaponCustomizationOptions>(builder.Configuration.GetSection("WeaponCustomization"));
            builder.Services.Configure<WeaponArchetypesOptions>(builder.Configuration.GetSection("WeaponArchetypes"));
            builder.Services.Configure<PotionConfigurationOptions>(builder.Configuration.GetSection("PotionConfiguration"));
            builder.Services.Configure<RelicConfigurationOptions>(builder.Configuration.GetSection("RelicConfiguration"));
            builder.Services.Configure<MoxxTailConfigurationOptions>(builder.Configuration.GetSection("MoxxTailConfiguration"));
            builder.Services.Configure<TraumasConfigurationOptions>(builder.Configuration.GetSection("TraumasConfiguration"));
            builder.Services.Configure<ChestConfigurationOptions>(builder.Configuration.GetSection("ChestConfiguration"));
            builder.Services.Configure<ShieldCustomizationOptions>(builder.Configuration.GetSection("ShieldCustomization"));
            builder.Services.AddTransient<IItemFactory<GrenadeMod, GrenadeModFactoryParameters>, GrenadeModFactory>();
            builder.Services.AddTransient<IItemFactory<Shield, ShieldFactoryParameters>, ShieldFactory>();
            builder.Services.AddTransient<IItemFactory<Gun, GunRandomizerFactoryParameters>, GunFactory>();
            builder.Services.AddTransient<IItemFactory<Potion, BaseFactoryParameters>, PotionFactory>();
            builder.Services.AddTransient<IItemFactory<Relic, RelicRandomizerFactoryParameters>, RelicFactory>();
            builder.Services.AddTransient<IItemFactory<MoxxTail, BaseFactoryParameters>, MoxxTailFactory>();
            builder.Services.AddTransient<ITraumatizingService, TraumatizingService>();
            builder.Services.AddTransient<IPotionFinderService, PotionFinderService>();
            builder.Services.AddTransient<IChestService<DiceChestServiceParameters>, DiceChestService>();
            builder.Services.AddTransient<IChestService<UnassumingChestServiceParameters>, UnassumingChestService>();
            builder.Services.AddTransient<IChestService<CacheRollServiceParameters>, CacheRollService>();
            builder.Services.AddTransient<IChestService<EnemyDropServiceParameters>, EnemyDropService>();
            builder.Services.AddTransient<IGuildService, GuildService>();
            builder.Services.AddTransient<IGunBatchService, GunBatchService>();
            builder.Services.AddTransient<IShieldService, ShieldService>();
            builder.Services.AddTransient<IGrenadeModService, GrenadeModService>();
            builder.Services.AddTransient<IRelicService, RelicService>();
            builder.Services.AddTransient<IGunService, GunService>();
            builder.Services.AddTransient<IMoxxTailService, MoxxTailService>();
            builder.Services.AddSingleton<IWeaponCustomizationService, WeaponCustomizationService>();


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
        }
    }
}