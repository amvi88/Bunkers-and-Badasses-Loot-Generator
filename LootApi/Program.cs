using Business.Factories;
using Business.Services;
using Models.Builder;
using Models.Config;
using Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
AddGameConfiguration(builder);

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddValidation();

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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    StatusCodeSelector = exception => exception is BadHttpRequestException badRequest
        ? badRequest.StatusCode
        : StatusCodes.Status500InternalServerError,
});
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("HealthCheck");

var api = app.MapGroup("/api");

api.MapPost("/guns/randomize", (GunRandomizerFactoryParameters parameters, IGunService service) =>
    Results.Ok(service.RandomizeGun(parameters)))
    .WithName("RandomizeGun");

api.MapPost("/grenade-mods/randomize", (GrenadeModFactoryParameters parameters, IGrenadeModService service) =>
    Results.Ok(service.RandomizeGrenadeMod(parameters)))
    .WithName("RandomizeGrenadeMod");

api.MapPost("/shields/randomize", (ShieldFactoryParameters parameters, IShieldService service) =>
    Results.Ok(service.RandomizeShield(parameters)))
    .WithName("RandomizeShield");

api.MapPost("/relics/randomize", (RelicRandomizerFactoryParameters parameters, IRelicService service) =>
    Results.Ok(service.RandomizeRelic(parameters)))
    .WithName("RandomizeRelic");

api.MapPost("/spells/randomize", (SpellRandomizerFactoryParameters parameters, ISpellService service) =>
    Results.Ok(service.RandomizeSpell(parameters)))
    .WithName("RandomizeSpell");

api.MapGet("/moxx-tails/randomize", (IMoxxTailService service) =>
    Results.Ok(service.RandomizeDrink(new BaseFactoryParameters())))
    .WithName("RandomizeMoxxTail");

api.MapPost("/traumas/randomize", (TraumaServiceParameters parameters, ITraumatizingService service) =>
    Results.Ok(service.GetTrauma(parameters)))
    .WithName("GetTrauma");

api.MapPost("/guns/batch", (GunBatchServiceParameters parameters, IGunBatchService service) =>
    Results.Ok(service.GenerateBatch(parameters)))
    .WithName("GenerateGunBatch");

api.MapPost("/shields/batch", (ShieldBatchServiceParameters parameters, IShieldBatchService service) =>
    Results.Ok(service.GenerateBatch(parameters)))
    .WithName("GenerateShieldBatch");

api.MapPost("/shops/inventory", (ShopServiceParameters parameters, IShopService service) =>
    Results.Ok(service.GenerateInventory(parameters)))
    .WithName("GenerateShopInventory");

api.MapPost("/chests/dice", (DiceChestServiceParameters parameters, IChestService<DiceChestServiceParameters> service) =>
    Results.Ok(service.OpenChest(parameters)))
    .WithName("OpenDiceChest");

api.MapPost("/chests/unassuming", (UnassumingChestServiceParameters parameters, IChestService<UnassumingChestServiceParameters> service) =>
    Results.Ok(service.OpenChest(parameters)))
    .WithName("OpenUnassumingChest");

api.MapPost("/chests/cache-roll", (CacheRollServiceParameters parameters, IChestService<CacheRollServiceParameters> service) =>
    Results.Ok(service.OpenChest(parameters)))
    .WithName("OpenCacheRoll");

api.MapPost("/chests/enemy-drop", (EnemyDropServiceParameters parameters, IChestService<EnemyDropServiceParameters> service) =>
    Results.Ok(service.OpenChest(parameters)))
    .WithName("OpenEnemyDrop");

api.MapGet("/customizations/prefixes", (ICustomizationService service) =>
    Results.Ok(service.GetPrefixes()))
    .WithName("GetWeaponPrefixes");

api.MapGet("/customizations/red-texts", (ICustomizationService service) =>
    Results.Ok(service.GetRedTexts()))
    .WithName("GetWeaponRedTexts");

api.MapGet("/customizations/weapon-archetype-bonuses", (ICustomizationService service) =>
    Results.Ok(service.GetWeaponArchetypeBonuses()))
    .WithName("GetWeaponArchetypeBonuses");

api.MapGet("/customizations/weapon-archetype-hits", (Models.Common.GunType gunType, int playerLevel, ICustomizationService service) =>
    Results.Ok(service.GetWeaponArchetypeHits(gunType, playerLevel)))
    .WithName("GetWeaponArchetypeHits");

api.MapPost("/potions/resolve", (ChestPotionSpec potionSpec, IPotionFinderService service) =>
{
    var potion = service.GetPotion(potionSpec);
    return potion is null ? Results.NotFound() : Results.Ok(potion);
})
    .WithName("ResolvePotion");

api.MapGet("/guilds", (IGuildService service) =>
    Results.Ok(service.GetGuilds()))
    .WithName("GetGuilds");

api.MapGet("/guilds/producing", (Models.Common.ItemType itemType, IGuildService service) =>
    Results.Ok(service.GetGuildsThatProduce(itemType)))
    .WithName("GetGuildsProducingItemType");

api.MapGet("/guilds/producing-guns", (Models.Common.GunType? gunType, IGuildService service) =>
    Results.Ok(service.GetGuildsThatProduceGunType(gunType)))
    .WithName("GetGuildsProducingGunType");

api.MapGet("/guilds/{guildName}/weapon-modifiers", (string guildName, Models.Common.Rarity rarity, IGuildService service) =>
{
    var modifiers = service.GetWeaponModifiers(guildName, rarity);
    return modifiers is null ? Results.NotFound() : Results.Ok(modifiers);
})
    .WithName("GetGuildWeaponModifiers");

app.Run();

static void AddGameConfiguration(WebApplicationBuilder builder)
{
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
    var configurationProvider = new PhysicalFileProvider(
        Path.Combine(AppContext.BaseDirectory, "Configuration"));

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
