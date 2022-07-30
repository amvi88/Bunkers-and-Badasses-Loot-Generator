using Models.Builder;
using Models.Common;
using Models.Config;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class GunFactory : GuildDrivenFactory<Gun, GunRandomizerFactoryParameters>
    {
        private readonly WeaponCustomizationOptions _weaponCustomization;
        private readonly WeaponArchetypesOptions _weaponArchetypesOptions;

        public GunFactory(IOptions<GuildConfigurationOptions> guildOptions, IOptions<WeaponCustomizationOptions> weaponCustomizationOptions, IOptions<WeaponArchetypesOptions> weaponArchetypesOptions)
            : base(guildOptions)
        {
            _weaponCustomization = weaponCustomizationOptions.Value ?? throw new ArgumentException(nameof(weaponCustomizationOptions));
            _weaponArchetypesOptions = weaponArchetypesOptions.Value ?? throw new ArgumentNullException(nameof(weaponArchetypesOptions));
        }
       
        public override ItemWrapper<Gun> Manufacture(GunRandomizerFactoryParameters builderArguments)
        {
            
            var rarity = GetRarity(builderArguments.Rarity);  
            var gunType = GetGunType(builderArguments.GunType);
            var chosenGuild = GetGuild(builderArguments.Guild, ItemType.Gun, gunType, out var roll);
             
            // Create a gun based on Guild specs
            var specs = chosenGuild.WeaponSpecs.First(x => x.Rarity == rarity);

            var gun = new Gun
            {
                Level = builderArguments.PlayerLevel,
                Guild =  chosenGuild.Name,
                Element = Element.None,
                Rarity = rarity,
                GuildBonus = specs.Bonus,
                GunType = gunType                         
            };

            // Roll for elemental type if any and bonus damage if it may be
            if (specs.IsElemental)
            {
                var elementalValues = Enum.GetValues(typeof(Element));

                var elementValues = RollElement(rarity, specs.IncreasedElementalRollPercentage);
                gun.Element = elementValues.Item1 ?? Element.None;

                if (! string.IsNullOrWhiteSpace(elementValues.Item2) && elementValues.Item1 != Element.None)
                {
                    gun.ElementalBonus = $"+{elementValues.Item2} {elementValues.Item1} DMG";
                    gun.ExtraDamage = elementValues.Item2;
                }
            }

            // Get archetype specs for weapons
            var archetype = _weaponArchetypesOptions.Archetypes.First(x => x.GunType == gunType);
            if (!string.IsNullOrEmpty(archetype.Bonus))
            {
                gun.GunArchetypeBonus = archetype.Bonus;
            }

            GetName(gun, archetype);

            var gunStats = archetype.WeaponSpecs.First(x => x.MinLevel <= builderArguments.PlayerLevel && builderArguments.PlayerLevel <= x.MaxLevel);
            gun.Range  = gunStats.Range;
            gun.Damage = gunStats.Damage;
            gun.HitsByAccuracy = gunStats.HitsByAccuracy;  
            
            // Check for prefix
            if (builderArguments.AllowPrefixes)
            {
                var calculatedPrefixChance = RandomNumberGenerator.GetInt32(1, 100);
                if (calculatedPrefixChance <= _weaponCustomization.PrefixChance[rarity])
                {
                    var prefix = _weaponCustomization.Prefixes[RandomNumberGenerator.GetInt32(0, _weaponCustomization.Prefixes.Length)];
                    gun.Prefix = prefix;
                }
            }

            if (builderArguments.AllowRedTexts)
            {
                var calculatedRedTextChance = RandomNumberGenerator.GetInt32(1, 100);
                if (calculatedRedTextChance  <= _weaponCustomization.RedTextChance[rarity])
                {
                    var redText = _weaponCustomization.RedText[RandomNumberGenerator.GetInt32(0, _weaponCustomization.RedText.Length)];
                    gun.RedText = redText;

                    if (redText.Element != null)
                    {
                        gun.Element |= redText.Element.Value;
                    }
                }
            }

            return new ItemWrapper<Gun>
            {
                DiceRolls = new DiceRoll[]
                {
                    new DiceRoll
                    {
                        DiceType = "d8",
                        Result = roll,
                        Purpose = "Manufacturer"
                    }
                },
                Item = gun
            };
        }

        private void GetName(Gun gun, WeaponArchetype archetype)
        {
            if (gun.Rarity != Rarity.Common)
            {
                var matchingWeapons = _weaponCustomization.WeaponGallery.Where(x => x.Guild == gun.Guild && x.GunType == gun.GunType.ToString() && x.Rarity == gun.Rarity );

                if (matchingWeapons.Any())
                {
                    var weaponGallerySpec = matchingWeapons.ElementAt(RandomNumberGenerator.GetInt32(0, matchingWeapons.Count()));
                    gun.Name = weaponGallerySpec.Name;
                    gun.ImageUrl = weaponGallerySpec.ImageUrl;
                    gun.Source = weaponGallerySpec.Source;
                    return;
                }
            }

            gun.Name = archetype.Names[RandomNumberGenerator.GetInt32(0, archetype.Names.Length)];
        }

        private (Element?, string) RollElement(Rarity? rarity, int? rollModifier)
        {
            var percentileRoll = Math.Min(RandomNumberGenerator.GetInt32(1, 101)+rollModifier.GetValueOrDefault() , 100);
            var elementalSpec = _weaponCustomization.ElementalTable.Where(x => x.MinxRoll <= percentileRoll && percentileRoll <= x.MaxRoll).First();
            var index = rarity == null? 0 : (int) rarity;
            return(elementalSpec.Elements[index], elementalSpec.ExtraDamage[index]);
        }

        private Rarity GetRarity(Rarity? rarity)
        {
            if (! rarity.HasValue)
            {
                var rowNumber = RandomNumberGenerator.GetInt32(0, _weaponCustomization.RarityMatrix.Length);
                var colNumber = RandomNumberGenerator.GetInt32(0, _weaponCustomization.RarityMatrix[0].Length);
                return _weaponCustomization.RarityMatrix[rowNumber][colNumber];
            }

            return rarity.GetValueOrDefault();
        }

        private GunType GetGunType(GunType? gunType)
        {
            if (! gunType.HasValue)
            {
                var gunTypeValues = Enum.GetValues(typeof(GunType));
                return  (GunType)gunTypeValues.GetValue(RandomNumberGenerator.GetInt32(1,gunTypeValues.Length));
            }

            return gunType.GetValueOrDefault();
        }
    }
}