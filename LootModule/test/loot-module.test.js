import test from 'node:test';
import assert from 'node:assert/strict';
import * as loot from '../index.js';

test('loads the shared configuration directory', () => {
    assert.match(loot.configurationPath, /Configuration$/);
    assert.deepEqual(loot.getHealth(), { status: 'ok' });
    assert.ok(loot.getGuilds().length > 0);
});

test('returns item wrappers for generated items', () => {
    const gun = loot.randomizeGun({ playerLevel: 10, guild: 'Alas!', rarity: 'Rare', gunType: 'Pistol' });
    assert.ok(Array.isArray(gun.diceRolls));
    assert.equal(typeof gun.item.name, 'string');
    assert.equal(gun.item.level, 10);

    const grenadeMod = loot.randomizeGrenadeMod({ playerLevel: 10, guild: 'Ashen' });
    assert.ok(Array.isArray(grenadeMod.diceRolls));
    assert.equal(grenadeMod.item.level, 10);

    assert.ok(loot.randomizeShield({ playerLevel: 10, guild: 'Ashen' }).item);
    assert.ok(loot.randomizeRelic().item);
    assert.ok(loot.randomizeSpell({ playerLevel: 10, guild: 'Conjura' }).item);
    assert.ok(loot.randomizeMoxxTail().item);
});

test('exposes local lookup and batch operations', () => {
    assert.ok(Object.keys(loot.getPrefixes()).length > 0);
    assert.ok(Object.keys(loot.getRedTexts()).length > 0);
    assert.ok(Object.hasOwn(loot.getWeaponArchetypeBonuses(), 'Pistol'));
    assert.ok(loot.getWeaponArchetypeHits({ gunType: 'Pistol', playerLevel: 10 }).length > 0);
    assert.equal(loot.generateGunBatch({ playerLevel: 10, batchSize: 2 }).length, 2);
    assert.equal(loot.generateShieldBatch({ playerLevel: 10, batchSize: 2 }).length, 2);
    assert.ok(loot.generateShopInventory({ playerLevel: 10 }).length > 0);
    assert.ok(loot.getGuildsProducing({ itemType: 'Shield' }).length > 0);
    assert.ok(loot.getGuildsProducingGunType({ gunType: 'Pistol' }).length > 0);
    assert.ok(loot.getGuildWeaponModifiers({ guildName: 'Alas!', rarity: 'Rare' }));
});

test('resolves potions and creates trauma', () => {
    assert.equal(loot.resolvePotionSpec({ name: 'not configured' }), null);
    assert.ok(loot.resolvePotionSpec({ isRandomPotion: true }));
    assert.ok(loot.getTrauma({ traumaType: 'Permanent' }).length === 1);
});

test('opens configured chest and drop types', () => {
    assert.ok(Array.isArray(loot.openDiceChest({ playerLevel: 10 }).items));
    assert.ok(Array.isArray(loot.openUnassumingChest({ playerLevel: 10 }).items));
    assert.ok(Array.isArray(loot.openCacheRoll({ playerLevel: 10 }).items));
    assert.ok(Array.isArray(loot.openEnemyDrop({ playerLevel: 10, lootPiles: 1 }).items));
});

test('rejects invalid levels and enum values', () => {
    assert.throws(() => loot.randomizeGun({ playerLevel: 31 }), RangeError);
    assert.throws(() => loot.randomizeGun({ playerLevel: 10, gunType: 'Unknown' }), RangeError);
    assert.throws(() => loot.openEnemyDrop({ playerLevel: 10, lootPiles: 7 }), RangeError);
});
