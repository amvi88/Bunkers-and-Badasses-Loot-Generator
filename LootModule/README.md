# Local Loot ES Module

This package provides local Node.js functions for the same loot generation, chest, shop, guild, potion, trauma, and customization operations exposed by `LootApi`. It does not make HTTP requests. It reads the shared JSON files from the repository's root `Configuration/` directory when the module is imported.

## Requirements

- Node.js `^20.19.0` or `>=22.12.0`
- The repository's shared `Configuration/` directory, unless `LOOT_CONFIGURATION_DIRECTORY` is set before import

## Import

The package has no third-party dependencies. From the repository root:

```js
import {
  randomizeGun,
  openDiceChest,
  getGuilds,
} from './LootModule/index.js';

const gun = randomizeGun({
  playerLevel: 10,
  guild: 'Alas!',
  rarity: 'Rare',
  gunType: 'Pistol',
  allowPrefixes: true,
  allowRedTexts: true,
});

console.log(gun.item.name);
console.log(openDiceChest({ playerLevel: 10 }).items);
console.log(getGuilds());
```

The generator functions return synchronous values. Generated items follow the API's JSON response shapes, including `{ diceRolls, item }` wrappers where the API returns `ItemWrapper<T>`. Enum inputs are case-insensitive strings such as `"Rare"`, `"Pistol"`, `"Incendiary"`, and `"Temporary"`. Pass `playerLevel` values from 1 to 30. Invalid enum values and out-of-range values throw `RangeError`; missing or inconsistent configuration throws an `Error`.

## Exported functions

| Export | Purpose | Parameters |
|---|---|---|
| `getHealth()` | Return the API health response shape | None |
| `randomizeGun(options)` | Generate a gun | `playerLevel`, optional `guild`, `rarity`, `gunType`, `allowPrefixes`, `allowRedTexts` |
| `randomizeGrenadeMod(options)` | Generate a grenade mod | `playerLevel`, optional `guild`, `rarity`, `element` |
| `randomizeShield(options)` | Generate a shield | `playerLevel`, optional `guild` |
| `randomizeRelic(options)` | Generate a relic | Optional `rarity` |
| `randomizeSpell(options)` | Generate a spell | `playerLevel`, optional `guild`, `rarity` |
| `randomizeMoxxTail()` | Generate a Moxx-tail | None |
| `getTrauma(options)` | Generate temporary and/or permanent trauma | Optional `traumaType` (`Temporary` or `Permanent`) |
| `generateGunBatch(options)` | Generate a batch of guns | `playerLevel`, optional `batchSize` (1-32) |
| `generateShieldBatch(options)` | Generate a batch of shields | `playerLevel`, optional `batchSize` (1-32) |
| `generateShopInventory(options)` | Generate a shop inventory | `playerLevel`, optional `amountOfGuns`, `amountOfGrenadeMods`, `amountOfShieldMods`, `amountOfPotions` (each 1-32) |
| `openDiceChest(options)` | Open a dice chest | `playerLevel`, optional `gunType`, `allowPrefixes`, `allowRedTexts` |
| `openUnassumingChest(options)` | Open an unassuming chest | `playerLevel`, optional `gunType`, `allowPrefixes`, `allowRedTexts` |
| `openCacheRoll(options)` | Resolve a cache roll | `playerLevel`, optional `cacheRollSize` (`Tiny` through `Legendary`), `goldFarmerBonus`, `gunType`, prefix options |
| `openEnemyDrop(options)` | Resolve one or more enemy loot piles | `playerLevel`, optional `lootPiles` (1-6), `gunType`, prefix options |
| `getPrefixes()` | Get the weapon-prefix dictionary | None |
| `getRedTexts()` | Get the weapon red-text dictionary | None |
| `getWeaponArchetypeBonuses()` | Get weapon-type bonuses | None |
| `getWeaponArchetypeHits(options)` | Get hits by accuracy for a type and level | `gunType`, `playerLevel` |
| `resolvePotionSpec(spec)` | Resolve a named or random potion | `isRandomPotion`, `name` |
| `getGuilds()` | Get all guilds | None |
| `getGuildsProducing(options)` | Filter guilds by item type | `itemType` |
| `getGuildsProducingGunType(options)` | Filter guilds by gun type | Optional `gunType` |
| `getGuildWeaponModifiers(options)` | Get rarity-specific guild modifiers | `guildName`, `rarity` |

Optional guild names accept either the guild's primary or alternate name where item generation supports guild selection. Unknown named potions resolve to `null`; absent guild/rarity modifier combinations return `null`.

The legacy gun factory accepts an `element` request property but does not use it to force the generated element; elemental results are determined by the rarity and shared elemental configuration, matching the API's current behavior.

The module loads every shared configuration JSON file at import time. Set `LOOT_CONFIGURATION_DIRECTORY` to a different directory before starting Node.js to load an alternate complete set of configuration files.

## Tests

Run the local tests from this directory:

```shell
npm test
```
