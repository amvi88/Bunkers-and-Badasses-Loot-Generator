using Microsoft.AspNetCore.Http.Json;

using Business.Factories;
using Models.Config;
using Models.Builder;
using Models;
using Business.Services;
using ElectronNET.API;
using Microsoft.Extensions.FileProviders;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        SetupConfiguration(builder);

        builder.Logging.AddAzureWebAppDiagnostics();

        // Add services to the container.
        builder.Services.Configure<JsonOptions>(options =>
        {
            var enumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
            options.SerializerOptions.Converters.Add(enumConverter);
        });
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor()
        .AddCircuitOptions(options => { options.DetailedErrors = true; });

        
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
        builder.Services.AddSingleton<ICustomizationService, CustomizationService>();
        builder.Services.AddBlazorContextMenu();

        builder.Services.AddElectron();
        builder.WebHost.UseElectron(args);

        if (HybridSupport.IsElectronActive)
        {
            // Open the Electron-Window here
            Task.Run(async () => {
                var window = await Electron.WindowManager.CreateWindowAsync();
                window.OnClosed += () => {
                    Electron.App.Quit();
                };
            });
        }

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
    }

    private static void SetupConfiguration(WebApplicationBuilder builder)
    {
        var configurationProvider = new PhysicalFileProvider(
            Path.Combine(AppContext.BaseDirectory, "Configuration"));
        var configurationFiles = new[]
        {
            "weaponarchetypes.json",
            "weaponcustomization.json",
            "weaponprefixes.json",
            "weaponredtext.json",
            "weapongallery.json",
            "shieldgallery.json",
            "guilds.json",
            "potions.json",
            "tinytinaspotions.json",
            "relics.json",
            "moxxtails.json",
            "traumas.json",
            "dicechests.json",
            "unassumingchests.json",
            "cacherolls.json",
            "enemydrops.json",
            "spells.json",
            "general.json",
        };

        foreach (var fileName in configurationFiles)
        {
            builder.Configuration.AddJsonFile(
                configurationProvider,
                fileName,
                optional: false,
                reloadOnChange: true);
        }

        builder.Services.Configure<GuildConfigurationOptions>(builder.Configuration.GetSection("GuildConfiguration"));
        builder.Services.Configure<CustomizationOptions>(builder.Configuration.GetSection("WeaponCustomization"));
        builder.Services.Configure<WeaponArchetypesOptions>(builder.Configuration.GetSection("WeaponArchetypes"));
        builder.Services.Configure<PotionConfigurationOptions>(builder.Configuration.GetSection("PotionConfiguration"));
        builder.Services.Configure<RelicConfigurationOptions>(builder.Configuration.GetSection("RelicConfiguration"));
        builder.Services.Configure<MoxxTailConfigurationOptions>(builder.Configuration.GetSection("MoxxTailConfiguration"));
        builder.Services.Configure<TraumasConfigurationOptions>(builder.Configuration.GetSection("TraumasConfiguration"));
        builder.Services.Configure<ChestConfigurationOptions>(builder.Configuration.GetSection("ChestConfiguration"));
        builder.Services.Configure<ShieldCustomizationOptions>(builder.Configuration.GetSection("ShieldCustomization"));
        builder.Services.Configure<SpellConfigurationOptions>(builder.Configuration.GetSection("SpellConfiguration"));
    }
}