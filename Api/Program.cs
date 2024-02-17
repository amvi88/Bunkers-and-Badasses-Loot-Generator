
using Business.Factories;
using Business.Services;
using Models.Builder;
using Models.Config;
using Newtonsoft.Json.Converters;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuration.
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
            builder.Configuration.AddJsonFile("Configuration/spells.json", true);

            builder.Services.Configure<GuildConfigurationOptions>(builder.Configuration.GetSection("GuildConfiguration"));
            builder.Services.Configure<WeaponCustomizationOptions>(builder.Configuration.GetSection("WeaponCustomization"));
            builder.Services.Configure<WeaponArchetypesOptions>(builder.Configuration.GetSection("WeaponArchetypes"));
            builder.Services.Configure<PotionConfigurationOptions>(builder.Configuration.GetSection("PotionConfiguration"));
            builder.Services.Configure<RelicConfigurationOptions>(builder.Configuration.GetSection("RelicConfiguration"));
            builder.Services.Configure<MoxxTailConfigurationOptions>(builder.Configuration.GetSection("MoxxTailConfiguration"));
            builder.Services.Configure<TraumasConfigurationOptions>(builder.Configuration.GetSection("TraumasConfiguration"));
            builder.Services.Configure<ChestConfigurationOptions>(builder.Configuration.GetSection("ChestConfiguration"));
            builder.Services.Configure<ShieldCustomizationOptions>(builder.Configuration.GetSection("ShieldCustomization"));
            builder.Services.Configure<SpellConfigurationOptions>(builder.Configuration.GetSection("SpellConfiguration"));

            // Add services to the container.
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options => 
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGenNewtonsoftSupport();

            builder.Services.AddTransient<IItemFactory<GrenadeMod, GrenadeModFactoryParameters>, GrenadeModFactory>();
            builder.Services.AddTransient<IItemFactory<Shield, ShieldFactoryParameters>, ShieldFactory>();
            builder.Services.AddTransient<IItemFactory<Gun, GunRandomizerFactoryParameters>, GunFactory>();
            builder.Services.AddTransient<IItemFactory<Potion, BaseFactoryParameters>, PotionFactory>();
            builder.Services.AddTransient<IItemFactory<Relic, RelicRandomizerFactoryParameters>, RelicFactory>();
            builder.Services.AddTransient<IItemFactory<MoxxTail, BaseFactoryParameters>, MoxxTailFactory>();
            builder.Services.AddTransient<IItemFactory<Spell, SpellRandomizerFactoryParameters>, SpellFactory>();
            builder.Services.AddTransient<ITraumatizingService, TraumatizingService>();
            builder.Services.AddTransient<IPotionFinderService, PotionFinderService>();
            builder.Services.AddTransient<IChestService<DiceChestServiceParameters>, DiceChestService>();
            builder.Services.AddTransient<IChestService<UnassumingChestServiceParameters>, UnassumingChestService>();
            builder.Services.AddTransient<IChestService<CacheRollServiceParameters>, CacheRollService>();
            builder.Services.AddTransient<IChestService<EnemyDropServiceParameters>, EnemyDropService>();
            builder.Services.AddTransient<IGuildService, GuildService>();
            builder.Services.AddTransient<IGunBatchService, GunBatchService>();
            builder.Services.AddTransient<IShieldService, ShieldService>();
            builder.Services.AddTransient<IShieldBatchService, ShieldBatchService>();
            builder.Services.AddTransient<ISpellService, SpellService>();
            builder.Services.AddTransient<IShopService, ShopService>();
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