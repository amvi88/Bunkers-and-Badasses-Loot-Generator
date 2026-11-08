import { randomInt } from 'node:crypto';
import { readFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import path from 'node:path';

const defaultConfigurationDirectory = fileURLToPath(new URL('../Configuration/', import.meta.url));
const configurationDirectory = path.resolve(
    process.env.LOOT_CONFIGURATION_DIRECTORY ?? defaultConfigurationDirectory,
);

const configurationFiles = [
    'cacherolls.json',
    'dicechests.json',
    'enemydrops.json',
    'general.json',
    'guilds.json',
    'moxxtails.json',
    'potions.json',
    'relics.json',
    'shieldgallery.json',
    'spells.json',
    'tinytinaspotions.json',
    'traumas.json',
    'unassumingchests.json',
    'weaponarchetypes.json',
    'weaponcustomization.json',
    'weapongallery.json',
    'weaponprefixes.json',
    'weaponredtext.json',
];

const config = Object.create(null);
for (const fileName of configurationFiles) {
    const filePath = path.join(configurationDirectory, fileName);
    let json;
    try {
        json = JSON.parse(readFileSync(filePath, 'utf8'));
    } catch (error) {
        throw new Error(`Failed to load loot configuration "${filePath}": ${error.message}`, { cause: error });
    }

    for (const [sectionName, sections] of Object.entries(json)) {
        config[sectionName] = { ...config[sectionName], ...sections };
    }
}

const rarityValues = ['Common', 'Uncommon', 'Rare', 'Epic', 'Legendary', 'Pearlescent'];
const elementValues = ['None', 'Incendiary', 'Shock', 'Explosive', 'Corrosive', 'Radiation', 'Cryo'];
const gunTypeValues = ['CombatRifle', 'Pistol', 'SubmachineGun', 'Shotgun', 'SniperRifle', 'RocketLauncher'];
const itemTypeValues = ['Shield', 'Grenade', 'Gun', 'Spell', 'MeleeWeapon'];
const spellTypeValues = ['ArcTorrent', 'Sigil', 'Eruption', 'Fireball', 'Sunder', 'Talon', 'Hydra', 'IceSpike', 'Barrage', 'Calamity'];
const traumaTypeValues = ['Temporary', 'Permanent'];
const cacheRollSizeValues = ['Tiny', 'Small', 'Medium', 'Large', 'Legendary'];

const enumBits = (values) => Object.fromEntries(values.map((value, index) => [value, 1 << index]));
const gunTypeBits = enumBits(gunTypeValues);
const itemTypeBits = enumBits(itemTypeValues);
const elementBits = { None: 0, ...enumBits(elementValues.slice(1)) };
const spellTypeBits = enumBits(spellTypeValues);

function randomBetween(minimum, maximum) {
    return randomInt(minimum, maximum + 1);
}

function randomChoice(values) {
    if (!Array.isArray(values) || values.length === 0) {
        throw new Error('Cannot choose a random value from an empty collection.');
    }
    return values[randomInt(values.length)];
}

function value(object, name, fallback = undefined) {
    if (object == null) return fallback;
    const key = Object.keys(object).find((candidate) => candidate.toLowerCase() === name.toLowerCase());
    return key === undefined ? fallback : object[key];
}

function section(name, child) {
    const result = value(config, name);
    if (result === undefined) throw new Error(`Missing "${name}" section in the shared configuration.`);
    return child === undefined ? result : value(result, child);
}

function camelize(input) {
    if (Array.isArray(input)) return input.map(camelize);
    if (input === null || typeof input !== 'object') return input;
    return Object.fromEntries(Object.entries(input).map(([key, item]) => [
        key.length === 0 ? key : key[0].toLowerCase() + key.slice(1),
        camelize(item),
    ]));
}

function enumName(input, values, fieldName) {
    if (typeof input !== 'string') {
        throw new TypeError(`${fieldName} must be one of: ${values.join(', ')}.`);
    }
    const result = values.find((candidate) => candidate.toLowerCase() === input.trim().toLowerCase());
    if (result === undefined) throw new RangeError(`Unknown ${fieldName} "${input}". Expected one of: ${values.join(', ')}.`);
    return result;
}

function enumFlag(input, values, bits, fieldName, fallback = undefined) {
    if (input === undefined || input === null || input === '') {
        if (fallback !== undefined) return { names: [fallback], value: bits[fallback] ?? 0 };
        return { names: [], value: 0 };
    }
    const requested = Array.isArray(input) ? input : String(input).split(',');
    let numericValue = 0;
    for (const rawName of requested) {
        const name = enumName(String(rawName).trim(), values, fieldName);
        numericValue |= bits[name] ?? 0;
    }
    return {
        names: values.filter((name) => (numericValue & (bits[name] ?? 0)) !== 0),
        value: numericValue,
    };
}

function parseFlagNames(input, values, fieldName) {
    if (input === undefined || input === null || input === '') return [];
    return enumFlag(input, values, enumBits(values), fieldName).names;
}

function flagsContain(names, targetValue, bits) {
    const supportedValue = names.reduce((result, name) => result | (bits[name] ?? 0), 0);
    return targetValue !== 0 && (supportedValue & targetValue) === targetValue;
}

function enumString(input, values, bits, fieldName, defaultValue) {
    if (typeof input === 'number') {
        const names = values.filter((name) => {
            const bit = bits[name] ?? 0;
            return bit !== 0 && (input & bit) !== 0;
        });
        return names.length === 0 ? (defaultValue ?? '') : names.join(', ');
    }
    const parsed = enumFlag(input, values, bits, fieldName, defaultValue);
    return parsed.names.length === 0 ? (defaultValue ?? '') : parsed.names.join(', ');
}

function requireIntegerInRange(input, fieldName, minimum, maximum, defaultValue) {
    const result = input === undefined ? defaultValue : input;
    if (!Number.isInteger(result) || result < minimum || result > maximum) {
        throw new RangeError(`${fieldName} must be an integer from ${minimum} to ${maximum}.`);
    }
    return result;
}

function validateBoolean(input, fieldName) {
    if (input !== undefined && typeof input !== 'boolean') {
        throw new TypeError(`${fieldName} must be a boolean.`);
    }
}

function validateChestParameters(parameters) {
    levelOf(parameters);
    enumFlag(parameters.gunType ?? 'CombatRifle', gunTypeValues, gunTypeBits, 'gunType');
    validateBoolean(parameters.allowPrefixes, 'allowPrefixes');
    validateBoolean(parameters.allowRedTexts, 'allowRedTexts');
}

function levelOf(parameters) {
    return requireIntegerInRange(parameters.playerLevel, 'playerLevel', 1, 30, 1);
}

function matchingLevel(specifications, level, context) {
    const result = specifications?.find((spec) => value(spec, 'MinLevel') <= level && level <= value(spec, 'MaxLevel'));
    if (!result) throw new Error(`No ${context} configuration exists for player level ${level}.`);
    return result;
}

function lookupGuild(guildName, itemType, gunType = undefined) {
    const requestedItem = itemTypeValues.find((name) => name.toLowerCase() === itemType.toLowerCase());
    if (!requestedItem) throw new RangeError(`Unknown item type "${itemType}".`);
    const itemBit = itemTypeBits[requestedItem];
    const gunBit = gunType === undefined ? undefined : gunTypeBits[gunType];
    const guilds = section('GuildConfiguration', 'Guilds').filter((guild) => {
        const supportedItems = parseFlagNames(value(guild, 'SupportedItemTypes'), itemTypeValues, 'SupportedItemTypes');
        const supportedGuns = parseFlagNames(value(guild, 'SupportedGunTypes'), gunTypeValues, 'SupportedGunTypes');
        const producesItem = flagsContain(supportedItems, itemBit, itemTypeBits);
        const producesGun = gunBit === undefined || flagsContain(supportedGuns, gunBit, gunTypeBits);
        return producesItem && producesGun;
    });

    if (guildName !== undefined && guildName !== null && String(guildName).trim() !== '') {
        const normalized = String(guildName).trim().toLocaleLowerCase();
        const found = guilds.find((guild) =>
            String(value(guild, 'Name', '')).toLocaleLowerCase() === normalized
            || String(value(guild, 'AlternateName', '')).toLocaleLowerCase() === normalized);
        if (!found) throw new RangeError(`No guild named "${guildName}" produces ${gunType ?? requestedItem}.`);
        return { guild: found, roll: 0 };
    }

    if (guilds.length === 0) throw new Error(`No guilds produce ${gunType ?? requestedItem}.`);
    const index = randomInt(guilds.length);
    return { guild: guilds[index], roll: index + 1 };
}

function randomRarity(requested) {
    if (requested !== undefined && requested !== null) return enumName(requested, rarityValues, 'rarity');
    const matrix = section('WeaponCustomization', 'RarityMatrix');
    return randomChoice(randomChoice(matrix));
}

function rarityIndex(rarity) {
    return rarityValues.indexOf(rarity);
}

function getWeaponStats(gunType, playerLevel) {
    const archetype = section('WeaponArchetypes', 'Archetypes')
        .find((item) => String(value(item, 'GunType')).toLowerCase() === gunType.toLowerCase());
    if (!archetype) throw new Error(`No weapon archetype is configured for ${gunType}.`);
    const stats = matchingLevel(value(archetype, 'WeaponSpecs'), playerLevel, `${gunType} weapon`);
    return { archetype, stats };
}

function rollElement(rarity, rollModifier = 0) {
    const percentile = Math.min(randomBetween(1, 100) + rollModifier, 100);
    const spec = section('WeaponCustomization', 'ElementalTable').find((entry) =>
        (value(entry, 'MinxRoll') ?? 0) <= percentile && percentile <= value(entry, 'MaxRoll'));
    if (!spec) throw new Error(`No elemental configuration exists for percentile roll ${percentile}.`);
    const index = rarityIndex(rarity);
    return {
        element: value(spec, 'Elements')[index] ?? 'None',
        extraDamage: value(spec, 'ExtraDamage')[index] ?? '',
    };
}

function randomGunType() {
    return gunTypeValues[randomInt(1, gunTypeValues.length)];
}

function galleryName(guildName, gunType, rarity, archetype) {
    if (rarity !== 'Common') {
        const matchingWeapons = section('WeaponCustomization', 'WeaponGallery').filter((weapon) =>
            value(weapon, 'Guild') === guildName
            && value(weapon, 'GunType') === gunType
            && value(weapon, 'Rarity') === rarity);
        if (matchingWeapons.length > 0) {
            const weapon = randomChoice(matchingWeapons);
            return {
                name: value(weapon, 'Name'),
                imageUrl: value(weapon, 'ImageUrl') ?? null,
                source: value(weapon, 'Source') ?? null,
            };
        }
    }

    return { name: randomChoice(value(archetype, 'Names')), imageUrl: null, source: null };
}

function createGun(parameters = {}) {
    const playerLevel = levelOf(parameters);
    requireIntegerInRange(parameters.batchSize, 'batchSize', 1, 100, 1);
    validateBoolean(parameters.allowPrefixes, 'allowPrefixes');
    validateBoolean(parameters.allowRedTexts, 'allowRedTexts');
    validateBoolean(parameters.batchMode, 'batchMode');
    const rarity = randomRarity(parameters.rarity);
    const gunType = parameters.gunType == null
        ? randomGunType()
        : enumName(parameters.gunType, gunTypeValues, 'gunType');
    if (parameters.element != null) {
        enumFlag(parameters.element, elementValues, elementBits, 'element');
    }
    const { guild, roll } = lookupGuild(parameters.guild, 'Gun', gunType);
    const weaponSpec = value(guild, 'WeaponSpecs')?.find((spec) => value(spec, 'Rarity') === rarity);
    if (!weaponSpec) throw new Error(`Guild "${value(guild, 'Name')}" has no ${rarity} weapon specification.`);

    const { archetype, stats } = getWeaponStats(gunType, playerLevel);
    const nameDetails = galleryName(value(guild, 'Name'), gunType, rarity, archetype);
    let element = 'None';
    let extraDamage = null;
    let elementalBonus = null;
    if (value(weaponSpec, 'IsElemental')) {
        let rolled = rollElement(rarity, value(weaponSpec, 'IncreasedElementalRollPercentage') ?? 0);
        if (value(weaponSpec, 'MustBeElemental')) {
            for (let tries = 0; rolled.element === 'None' && tries < 29; tries += 1) {
                rolled = rollElement(rarity, value(weaponSpec, 'IncreasedElementalRollPercentage') ?? 0);
            }
        }
        element = rolled.element;
        if (rolled.extraDamage !== '' && element !== 'None') {
            extraDamage = rolled.extraDamage;
            elementalBonus = `+${rolled.extraDamage} ${element} DMG`;
        }
    }

    let prefix = null;
    if (parameters.allowPrefixes) {
        const chances = section('WeaponCustomization', 'PrefixChance');
        if (randomBetween(1, 100) <= value(chances, rarity, 0)) {
            prefix = camelize(randomChoice(section('WeaponCustomization', 'Prefixes')));
        }
    }

    let redText = null;
    if (parameters.allowRedTexts) {
        const chances = section('WeaponCustomization', 'RedTextChance');
        if (randomBetween(1, 100) <= value(chances, rarity, 0)) {
            const text = randomChoice(section('WeaponCustomization', 'RedText'));
            redText = camelize(text);
            const redTextElement = value(text, 'Element');
            if (redTextElement != null) {
                const redElement = enumFlag(redTextElement, elementValues, elementBits, 'element');
                const existingElement = enumFlag(element, elementValues, elementBits, 'element');
                element = enumString(existingElement.value | redElement.value, elementValues, elementBits, 'element', 'None');
            }
        }
    }

    const damage = value(stats, 'Damage');
    const extraDamageParts = String(extraDamage ?? '');
    const extraDiceCount = Number.parseInt(extraDamageParts.slice(0, 1), 10);
    return {
        diceRolls: [{ purpose: null, diceType: 'd8', result: roll }],
        item: {
            guild: value(guild, 'Name'),
            alternateGuildName: value(guild, 'AlternateName') ?? null,
            imageUrl: nameDetails.imageUrl,
            rarity,
            element,
            level: playerLevel,
            damage,
            amountOfDice: Number.parseInt(String(damage ?? '').slice(0, 1), 10) || 0,
            diceType: damage ? String(damage).slice(1) : '',
            extraAmountOfDice: Number.isNaN(extraDiceCount) ? 0 : extraDiceCount,
            extraDiceType: extraDamageParts ? extraDamageParts.slice(1) : '',
            range: value(stats, 'Range'),
            name: nameDetails.name,
            gunType,
            redText,
            tealText: null,
            prefix,
            gunArchetypeBonus: value(archetype, 'Bonus') || null,
            elementalBonus,
            guildBonus: value(weaponSpec, 'Bonus') ?? null,
            hitsByAccuracy: camelize(value(stats, 'HitsByAccuracy')),
            extraDamage,
            source: nameDetails.source,
        },
    };
}

function createGrenadeMod(parameters = {}) {
    const playerLevel = levelOf(parameters);
    if (parameters.rarity != null) enumName(parameters.rarity, rarityValues, 'rarity');
    const { guild, roll } = lookupGuild(parameters.guild, 'Grenade');
    const spec = matchingLevel(value(guild, 'GrenadeSpecs'), playerLevel, 'grenade');
    let element = 'None';
    if (parameters.element != null) {
        element = enumString(parameters.element, elementValues, elementBits, 'element', 'None');
    } else if (value(spec, 'IsElemental')) {
        element = randomElement();
    }
    const damage = value(spec, 'Damage');
    const dice = String(damage).split(/[ +,]/).filter(Boolean);
    return {
        diceRolls: [{ purpose: null, diceType: 'd8', result: roll }],
        item: {
            guild: value(guild, 'Name'),
            alternateGuildName: value(guild, 'AlternateName') ?? null,
            imageUrl: null,
            level: playerLevel,
            damage,
            amountOfDice: dice.map((part) => Number.parseInt(part.slice(0, 1), 10)),
            diceType: dice.map((part) => part.slice(1)),
            grenadeType: value(spec, 'GrenadeType'),
            effect: value(spec, 'Effect'),
            element,
            rarity: 'Common',
            redText: null,
        },
    };
}

function randomElement() {
    return elementValues[randomInt(1, elementValues.length)];
}

function createShield(parameters = {}) {
    const playerLevel = levelOf(parameters);
    const { guild, roll } = lookupGuild(parameters.guild, 'Shield');
    const spec = matchingLevel(value(guild, 'ShieldSpecs'), playerLevel, 'shield');
    const gallery = section('ShieldCustomization', 'ShieldGallery').filter((entry) =>
        String(value(entry, 'Guild')).toLocaleLowerCase() === String(value(guild, 'Name')).toLocaleLowerCase());
    const galleryEntry = gallery.length > 0 ? randomChoice(gallery) : null;
    return {
        diceRolls: [{ purpose: null, diceType: 'd8', result: roll }],
        item: {
            guild: value(guild, 'Name'),
            alternateGuildName: value(guild, 'AlternateName') ?? null,
            imageUrl: galleryEntry ? value(galleryEntry, 'ImageUrl') ?? null : null,
            level: playerLevel,
            name: galleryEntry ? value(galleryEntry, 'Name') ?? null : null,
            capacity: value(spec, 'Capacity'),
            rechargeRate: value(spec, 'RechargeRate'),
            effect: value(spec, 'Effect'),
            source: galleryEntry ? value(galleryEntry, 'Source') ?? null : null,
            rarity: 'Common',
            redText: null,
            element: 'None',
        },
    };
}

function createRelic(parameters = {}) {
    const requestedRarity = parameters.rarity == null
        ? null
        : enumName(parameters.rarity, rarityValues, 'rarity');
    const specs = section('RelicConfiguration', 'RelicSpecs');
    let minimum = 1;
    let maximum = 100;
    if (requestedRarity != null) {
        const matching = specs.filter((spec) => value(spec, 'Rarity') === requestedRarity);
        if (matching.length === 0) throw new Error(`No relics are configured for rarity ${requestedRarity}.`);
        minimum = Math.min(...matching.map((spec) => value(spec, 'MinRoll') ?? value(spec, 'Roll')));
        maximum = Math.max(...matching.map((spec) => value(spec, 'MaxRoll') ?? value(spec, 'Roll')));
    }
    const roll = randomBetween(minimum, maximum);
    const spec = specs.find((entry) =>
        (value(entry, 'MinRoll') <= roll && roll <= value(entry, 'MaxRoll'))
        || value(entry, 'Roll') === roll);
    if (!spec) throw new Error(`No relic configuration exists for roll ${roll}.`);
    return {
        diceRolls: [{ purpose: null, diceType: 'd100', result: roll }],
        item: {
            guild: null,
            alternateGuildName: null,
            imageUrl: null,
            type: value(spec, 'Type'),
            effect: value(spec, 'Effect'),
            classEffect: value(spec, 'ClassEffect') ?? null,
            rarity: value(spec, 'Rarity'),
            preferredClass: randomChoice(section('RelicConfiguration', 'Classes')),
            redText: null,
        },
    };
}

function createSpell(parameters = {}) {
    const playerLevel = levelOf(parameters);
    const rarity = randomRarity(parameters.rarity);
    const spellType = spellTypeValues[randomInt(1, spellTypeValues.length)];
    const { guild } = lookupGuild(parameters.guild, 'Spell');
    const effectSpec = matchingLevel(value(guild, 'SpellEffectSpecs'), playerLevel, 'guild spell effect');
    const guildSpell = value(guild, 'SpellSpecs')?.find((spec) =>
        String(value(spec, 'SpellType')).toLowerCase() === spellType.toLowerCase());
    if (!guildSpell) throw new Error(`Guild "${value(guild, 'Name')}" has no ${spellType} spell specification.`);
    const spellSpec = section('SpellConfiguration', 'Spells').find((spec) =>
        String(value(spec, 'SpellType')).toLowerCase() === spellType.toLowerCase());
    if (!spellSpec) throw new Error(`No ${spellType} spell configuration exists.`);
    const damageSpec = matchingLevel(value(spellSpec, 'SpellDamageSpecs'), playerLevel, 'spell damage');
    const damage = value(damageSpec, 'Damage');
    return {
        diceRolls: [],
        item: {
            guild: value(guild, 'Name'),
            alternateGuildName: value(guild, 'AlternateName') ?? null,
            imageUrl: null,
            name: spellType,
            prefix: null,
            level: playerLevel,
            damage,
            amountOfDice: Number.parseInt(String(damage ?? '').slice(0, 1), 10) || 0,
            diceType: damage ? String(damage).slice(1) : '',
            effect: value(effectSpec, 'Effect'),
            guildEffect: value(guildSpell, 'Effect'),
            source: 'wonderlands',
            rarity,
        },
    };
}

function createPotion() {
    const roll = randomBetween(1, 110);
    const spec = section('PotionConfiguration', 'RegularPotions').find((entry) =>
        value(entry, 'MinRoll') <= roll && roll <= value(entry, 'MaxRoll'));
    if (!spec) throw new Error(`No potion configuration exists for roll ${roll}.`);
    const potion = {
        guild: null,
        alternateGuildName: null,
        imageUrl: null,
        name: value(spec, 'Name'),
        effect: value(spec, 'Effect') ?? null,
        cost: value(spec, 'Cost'),
        isTinyTinasPotion: false,
        isElemental: false,
        element: 'None',
        rarity: value(spec, 'Rarity') ?? 'Common',
    };

    if (value(spec, 'IsElemental')) {
        const element = randomElement();
        potion.isElemental = true;
        potion.element = element;
        potion.effect = String(value(spec, 'Effect')).replaceAll('{0}', element);
    } else if (value(spec, 'RollTinasPotions')) {
        const index = randomInt(20) + (value(spec, 'RollModifier') ?? 0);
        const tinaPotion = section('PotionConfiguration', 'TinyTinasPotions')[index];
        if (!tinaPotion) throw new Error(`Tiny Tina's potion index ${index} is outside the configured range.`);
        potion.isTinyTinasPotion = true;
        potion.name = value(tinaPotion, 'Name');
        potion.effect = value(tinaPotion, 'Effect') ?? null;
    }

    return { diceRolls: [{ purpose: null, diceType: 'd100', result: roll }], item: potion };
}

function resolvePotion(spec = {}) {
    if (value(spec, 'IsRandomPotion', false)) return createPotion().item;
    const requestedName = value(spec, 'Name', '');
    const potion = section('PotionConfiguration', 'RegularPotions').find((entry) =>
        String(value(entry, 'Name')).toLocaleLowerCase() === String(requestedName).toLocaleLowerCase());
    if (!potion) return null;
    return {
        guild: null,
        alternateGuildName: null,
        imageUrl: null,
        name: value(potion, 'Name'),
        effect: value(potion, 'Effect') ?? null,
        cost: value(potion, 'Cost'),
        isTinyTinasPotion: false,
        isElemental: false,
        element: 'None',
        rarity: value(potion, 'Rarity') ?? 'Common',
    };
}

function createMoxxTail() {
    const spec = randomChoice(section('MoxxTailConfiguration', 'MoxxTails'));
    return {
        diceRolls: [],
        item: {
            guild: null,
            alternateGuildName: null,
            imageUrl: null,
            name: value(spec, 'Name'),
            effect: value(spec, 'Effect'),
        },
    };
}

function createTrauma(parameters = {}) {
    const traumaType = enumName(parameters.traumaType ?? 'Temporary', traumaTypeValues, 'traumaType');
    const result = [];
    if (traumaType === 'Temporary') {
        const temporary = section('TraumasConfiguration', 'Temporary');
        const index = randomInt(temporary.length);
        const spec = temporary[index];
        result.push({
            roll: index + 1,
            name: value(spec, 'Name') ?? null,
            effect: value(spec, 'Effect') ?? null,
            traumaType: 'Temporary',
        });
        if (!value(spec, 'RerollAsPermanent')) return result;
    }
    const permanent = section('TraumasConfiguration', 'Permanent');
    const index = randomInt(permanent.length);
    const spec = permanent[index];
    result.push({
        roll: index + 1,
        name: value(spec, 'Name') ?? null,
        effect: value(spec, 'Effect') ?? null,
        traumaType: 'Permanent',
    });
    return result;
}

function createGuildItem(guild) {
    return {
        name: value(guild, 'Name'),
        alternameName: value(guild, 'AlternateName') ?? null,
        supportedItemTypes: enumString(
            parseFlagNames(value(guild, 'SupportedItemTypes'), itemTypeValues, 'SupportedItemTypes'),
            itemTypeValues,
            itemTypeBits,
            'supportedItemTypes',
        ),
        supportedGunTypes: value(guild, 'SupportedGunTypes')
            ? enumString(parseFlagNames(value(guild, 'SupportedGunTypes'), gunTypeValues, 'SupportedGunTypes'),
                gunTypeValues, gunTypeBits, 'supportedGunTypes')
            : null,
    };
}

function getPrefixes() {
    return Object.fromEntries(section('WeaponCustomization', 'Prefixes').map((prefix) => [
        value(prefix, 'Name'),
        camelize(prefix),
    ]));
}

function getRedTexts() {
    return Object.fromEntries(section('WeaponCustomization', 'RedText').map((redText) => [
        value(redText, 'Name'),
        camelize(redText),
    ]));
}

function getWeaponArchetypeBonuses() {
    return Object.fromEntries(section('WeaponArchetypes', 'Archetypes').map((archetype) => [
        value(archetype, 'GunType'),
        value(archetype, 'Bonus') ?? null,
    ]));
}

function getWeaponArchetypeHits(parameters = {}) {
    const gunType = enumName(parameters.gunType, gunTypeValues, 'gunType');
    const playerLevel = requireIntegerInRange(parameters.playerLevel, 'playerLevel', 1, 30, undefined);
    const { stats } = getWeaponStats(gunType, playerLevel);
    return camelize(value(stats, 'HitsByAccuracy'));
}

function getGuilds() {
    return section('GuildConfiguration', 'Guilds').map(createGuildItem);
}

function getGuildsProducing(parameters = {}) {
    const itemType = enumFlag(parameters.itemType, itemTypeValues, itemTypeBits, 'itemType');
    return section('GuildConfiguration', 'Guilds')
        .filter((guild) => flagsContain(
            parseFlagNames(value(guild, 'SupportedItemTypes'), itemTypeValues, 'SupportedItemTypes'),
            itemType.value,
            itemTypeBits,
        ))
        .map(createGuildItem);
}

function getGuildsProducingGunType(parameters = {}) {
    const gunType = parameters.gunType == null
        ? null
        : enumFlag(parameters.gunType, gunTypeValues, gunTypeBits, 'gunType');
    return section('GuildConfiguration', 'Guilds')
        .filter((guild) => {
            const itemTypes = parseFlagNames(value(guild, 'SupportedItemTypes'), itemTypeValues, 'SupportedItemTypes');
            if (!itemTypes.includes('Gun')) return false;
            if (gunType == null) return true;
            return flagsContain(
                parseFlagNames(value(guild, 'SupportedGunTypes'), gunTypeValues, 'SupportedGunTypes'),
                gunType.value,
                gunTypeBits,
            );
        })
        .map(createGuildItem);
}

function getGuildWeaponModifiers(parameters = {}) {
    const guildName = String(parameters.guildName ?? '');
    const rarity = enumName(parameters.rarity, rarityValues, 'rarity');
    const guild = section('GuildConfiguration', 'Guilds').find((entry) =>
        String(value(entry, 'Name')).toLocaleLowerCase() === guildName.toLocaleLowerCase());
    const spec = value(guild, 'WeaponSpecs')?.find((entry) => value(entry, 'Rarity') === rarity);
    if (!spec) return null;
    return {
        isElemental: Boolean(value(spec, 'IsElemental')),
        effect: value(spec, 'Effect') ?? null,
        bonus: value(spec, 'Bonus') ?? null,
    };
}

function fillChest(spec, parameters = {}) {
    const playerLevel = levelOf(parameters);
    validateChestParameters(parameters);
    const items = [];
    if (value(spec, 'IsMimic')) items.push({ guild: null, alternateGuildName: null, imageUrl: null });
    if (value(spec, 'Gold') > 0) {
        items.push({ guild: null, alternateGuildName: null, imageUrl: null, amount: value(spec, 'Gold') });
    }
    for (const weaponSpec of value(spec, 'WeaponSpecs', [])) {
        items.push(createGun({
            playerLevel,
            rarity: value(weaponSpec, 'Rarity') ?? 'Common',
            gunType: parameters.useFavoredGunType && value(weaponSpec, 'IsFavored')
                ? parameters.gunType
                : undefined,
            allowPrefixes: Boolean(parameters.allowPrefixes),
            allowRedTexts: Boolean(parameters.allowPrefixes),
        }).item);
    }
    for (const potionSpec of value(spec, 'PotionSpecs', [])) {
        items.push(resolvePotion(potionSpec));
    }
    for (const relicSpec of value(spec, 'RelicSpecs', [])) {
        items.push(createRelic({ rarity: value(relicSpec, 'Rarity') }).item);
    }
    for (let index = 0; index < (value(spec, 'AmountOfGrenadeMods') ?? 0); index += 1) {
        items.push(createGrenadeMod({ playerLevel }).item);
    }
    for (let index = 0; index < (value(spec, 'AmountOfGrenades') ?? 0); index += 1) {
        items.push({ guild: null, alternateGuildName: null, imageUrl: null, amount: 1 });
    }
    for (let index = 0; index < (value(spec, 'AmountOfShieldMods') ?? 0); index += 1) {
        items.push(createShield({ playerLevel }).item);
    }
    return items;
}

function openDiceChest(parameters = {}) {
    const chestSpecs = section('ChestConfiguration', 'DiceChests');
    const roll = Math.max(randomBetween(1, chestSpecs.length), randomBetween(1, chestSpecs.length));
    const spec = chestSpecs.find((entry) => value(entry, 'Roll') === roll);
    if (!spec) throw new Error(`No dice chest configuration exists for roll ${roll}.`);
    return {
        roll,
        items: fillChest(spec, {
            ...parameters,
            gunType: parameters.gunType ?? 'CombatRifle',
            useFavoredGunType: true,
        }),
    };
}

function openUnassumingChest(parameters = {}) {
    const chestSpecs = section('ChestConfiguration', 'UnassumingChests');
    const roll = randomBetween(1, chestSpecs.length - 1);
    const spec = chestSpecs.find((entry) => value(entry, 'Roll') === roll);
    if (!spec) throw new Error(`No unassuming chest configuration exists for roll ${roll}.`);
    return {
        roll,
        items: fillChest(spec, {
            ...parameters,
            gunType: parameters.gunType ?? 'CombatRifle',
            useFavoredGunType: true,
        }),
    };
}

function openCacheRoll(parameters = {}) {
    const cacheRollSize = enumName(parameters.cacheRollSize ?? 'Tiny', cacheRollSizeValues, 'cacheRollSize');
    const size = cacheRollSizeValues.indexOf(cacheRollSize) + 1;
    const diceCount = Math.min(size + (parameters.goldFarmerBonus ? 2 : 0), 6);
    const roll = randomBetween(1, diceCount * 6);
    const spec = section('ChestConfiguration', 'CacheRolls').find((entry) => value(entry, 'Roll') === roll);
    if (!spec) throw new Error(`No cache roll configuration exists for roll ${roll}.`);
    return { roll, items: fillChest(spec, { ...parameters, useFavoredGunType: false }) };
}

function openEnemyDrop(parameters = {}) {
    const lootPiles = requireIntegerInRange(parameters.lootPiles, 'lootPiles', 1, 6, 1);
    const playerLevel = levelOf(parameters);
    validateChestParameters(parameters);
    const drops = section('ChestConfiguration', 'EnemyDrops');
    const items = [];
    for (let index = 0; index < lootPiles; index += 1) {
        const pile = drops[index];
        if (!pile) throw new Error(`No enemy drop configuration exists for loot pile ${index + 1}.`);
        const spec = randomChoice(pile);
        items.push(...fillChest(spec, { ...parameters, playerLevel, useFavoredGunType: false }));
    }
    return { roll: 0, items };
}

function generateGunBatch(parameters = {}) {
    const playerLevel = levelOf(parameters);
    const batchSize = requireIntegerInRange(parameters.batchSize, 'batchSize', 1, 32, 1);
    return Array.from({ length: batchSize }, () =>
        createGun({ playerLevel, allowPrefixes: true, allowRedTexts: true }).item);
}

function generateShieldBatch(parameters = {}) {
    const playerLevel = levelOf(parameters);
    const batchSize = requireIntegerInRange(parameters.batchSize, 'batchSize', 1, 32, 1);
    return Array.from({ length: batchSize }, () => createShield({ playerLevel }).item);
}

function generateShopInventory(parameters = {}) {
    const playerLevel = levelOf(parameters);
    requireIntegerInRange(parameters.batchSize, 'batchSize', 1, 32, 1);
    const counts = [
        ['amountOfGuns', 1, 32],
        ['amountOfGrenadeMods', 1, 32],
        ['amountOfShieldMods', 1, 32],
        ['amountOfPotions', 1, 32],
    ].map(([name, minimum, maximum]) =>
        requireIntegerInRange(parameters[name], name, minimum, maximum, 1));
    const [guns, grenadeMods, shields, potions] = counts;
    return [
        ...Array.from({ length: guns }, () => createGun({ playerLevel }).item),
        ...Array.from({ length: grenadeMods }, () => createGrenadeMod({ playerLevel }).item),
        ...Array.from({ length: shields }, () => createShield({ playerLevel }).item),
        ...Array.from({ length: potions }, () => createPotion().item),
    ];
}

export const configurationPath = configurationDirectory;

export function getHealth() {
    return { status: 'ok' };
}

export function randomizeGun(parameters = {}) {
    return createGun(parameters);
}

export function randomizeGrenadeMod(parameters = {}) {
    return createGrenadeMod(parameters);
}

export function randomizeShield(parameters = {}) {
    return createShield(parameters);
}

export function randomizeRelic(parameters = {}) {
    return createRelic(parameters);
}

export function randomizeSpell(parameters = {}) {
    return createSpell(parameters);
}

export function randomizeMoxxTail() {
    return createMoxxTail();
}

export function getTrauma(parameters = {}) {
    return createTrauma(parameters);
}

export { generateGunBatch, generateShieldBatch, generateShopInventory };
export { openDiceChest, openUnassumingChest, openCacheRoll, openEnemyDrop };
export { getPrefixes, getRedTexts, getWeaponArchetypeBonuses, getWeaponArchetypeHits };

export function resolvePotionSpec(spec = {}) {
    return resolvePotion(spec);
}

export { getGuilds, getGuildsProducing, getGuildsProducingGunType, getGuildWeaponModifiers };
