using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalItems
{
    public class VanillaBalanceGlobalItem : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            switch (item.type)
            {
                case ItemID.AdamantiteGlaive:
                case ItemID.ChlorophyteClaymore:
                case ItemID.ChlorophytePartisan:
                case ItemID.ChlorophyteSaber:
                case ItemID.ClingerStaff:
                case ItemID.CobaltNaginata:
                case ItemID.CopperShortsword:
                case ItemID.CursedFlames:
                case ItemID.DaedalusStormbow:
                case ItemID.DarkLance:
                case ItemID.ElfMelter:
                case ItemID.Excalibur:
                case ItemID.Flamelash:
                case ItemID.Flamethrower:
                case ItemID.FlareGun:
                case ItemID.FlowerofFrost:
                case ItemID.Gladius:
                case ItemID.GoldShortsword:
                case ItemID.Gungnir:
                case ItemID.HeatRay:
                case ItemID.IronShortsword:
                case ItemID.Keybrand:
                case ItemID.LaserMachinegun:
                case ItemID.LaserRifle:
                case ItemID.LeadShortsword:
                case ItemID.LightDisc:
                case ItemID.MushroomSpear:
                case ItemID.MythrilHalberd:
                case ItemID.NimbusRod:
                case ItemID.NorthPole:
                case ItemID.OrichalcumHalberd:
                case ItemID.PalladiumPike:
                case ItemID.PlatinumShortsword:
                case ItemID.PoisonStaff:
                case ItemID.RainbowRod:
                case ItemID.Ruler:
                case ItemID.Sandgun:
                case ItemID.ScourgeoftheCorruptor:
                case ItemID.SilverShortsword:
                case ItemID.Spear:
                case ItemID.StarCannon:
                case ItemID.StardustCellStaff:
                case ItemID.Starfury:
                case ItemID.SuperStarCannon:
                case ItemID.Terragrim:
                case ItemID.TheRottedFork:
                case ItemID.TinShortsword:
                case ItemID.TitaniumTrident:
                case ItemID.TragicUmbrella:
                case ItemID.Trident:
                case ItemID.TungstenShortsword:
                case ItemID.Umbrella:
                case ItemID.VampireKnives:
                case ItemID.WaterBolt:
                case ItemID.Zenith:
                case ItemID.FairyQueenMagicItem:
                case ItemID.EmpressBlade:
                case ItemID.FairyQueenRangedItem:
                case ItemID.PiercingStarlight:
                case ItemID.RainbowWhip:
                case ItemID.RainbowCrystalStaff:
                    return true;
                default:
                    return false;
            }
        }

        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.AdamantiteGlaive:
                    item.damage = 55;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    break;
                case ItemID.ChlorophyteClaymore:
                    item.useTime = 22;
                    item.useAnimation = 22;
                    item.autoReuse = true;
                    break;
                case ItemID.ChlorophytePartisan:
                    item.damage = 60;
                    item.useTime = 15;
                    item.useAnimation = 15;
                    break;
                case ItemID.ChlorophyteSaber:
                    item.damage = 65;
                    break;
                case ItemID.ClingerStaff:
                    item.damage = 60;
                    item.knockBack = 3f;
                    item.mana = 30;
                    break;
                case ItemID.CobaltNaginata:
                    item.damage = 45;
                    item.useTime = 22;
                    item.useAnimation = 22;
                    break;
                case ItemID.CopperShortsword:
                    item.damage = 10;
                    break;
                case ItemID.CursedFlames:
                    item.damage = 60;
                    item.mana = 8;
                    break;
                case ItemID.DaedalusStormbow:
                    item.damage = 33;
                    break;
                case ItemID.DarkLance:
                    item.useTime = 17;
                    item.useAnimation = 17;
                    break;
                case ItemID.ElfMelter:
                    item.damage = 65;
                    item.ArmorPenetration = 40;
                    break;
                case ItemID.Flamelash:
                    item.damage = 36;
                    item.mana = 8;
                    break;
                case ItemID.Flamethrower:
                    item.damage = 45;
                    item.ArmorPenetration = 25;
                    break;
                case ItemID.FlareGun:
                    item.damage = 16;
                    break;
                case ItemID.FlowerofFrost:
                    item.damage = 75;
                    break;
                case ItemID.Gladius:
                    item.damage = 30;
                    break;
                case ItemID.GoldShortsword:
                    item.damage = 24;
                    break;
                case ItemID.Gungnir:
                    item.damage = 75;
                    item.useTime = 12;
                    item.useAnimation = 12;
                    break;
                case ItemID.HeatRay:
                    item.damage = 100;
                    item.ArmorPenetration = 20;
                    item.useTime = 4;
                    item.useAnimation = 20;
                    item.reuseDelay = 30;
                    break;
                case ItemID.IronShortsword:
                    item.damage = 16;
                    break;
                case ItemID.Keybrand:
                    item.useTime = 16;
                    item.useAnimation = 16;
                    break;
                case ItemID.LaserMachinegun:
                    item.damage = 75;
                    item.knockBack = 4f;
                    item.ArmorPenetration = 15;
                    break;
                case ItemID.LaserRifle:
                    item.damage = 35;
                    item.ArmorPenetration = 10;
                    break;
                case ItemID.LeadShortsword:
                    item.damage = 18;
                    break;
                case ItemID.LightDisc:
                    item.damage = 70;
                    break;
                case ItemID.MushroomSpear:
                    item.useTime = 20;
                    item.useAnimation = 20;
                    break;
                case ItemID.MythrilHalberd:
                    item.damage = 47;
                    item.useTime = 21;
                    item.useAnimation = 21;
                    break;
                case ItemID.NimbusRod:
                    item.damage = 45;
                    break;
                case ItemID.NorthPole:
                    item.damage = 95;
                    break;
                case ItemID.OrichalcumHalberd:
                    item.damage = 48;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    break;
                case ItemID.PalladiumPike:
                    item.damage = 45;
                    item.useTime = 21;
                    item.useAnimation = 21;
                    break;
                case ItemID.PlatinumShortsword:
                    item.damage = 26;
                    break;
                case ItemID.PoisonStaff:
                    item.damage = 50;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.mana = 15;
                    break;
                case ItemID.RainbowRod:
                    item.damage = 70;
                    break;
                case ItemID.Ruler:
                    item.damage = 30;
                    break;
                case ItemID.Sandgun:
                    item.damage = 35;
                    item.shootSpeed *= 1.25f;
                    break;
                case ItemID.ScourgeoftheCorruptor:
                    item.damage = 85;
                    break;
                case ItemID.SilverShortsword:
                    item.damage = 18;
                    break;
                case ItemID.Spear:
                    item.damage = 15;
                    item.useTime = 28;
                    item.useAnimation = 28;
                    break;
                case ItemID.StakeLauncher:
                    item.damage = 85;
                    break;
                case ItemID.StarCannon:
                    item.damage = 50;
                    item.useTime = 6;
                    item.useAnimation = 18;
                    item.reuseDelay = 30;
                    break;
                case ItemID.SuperStarCannon:
                    item.useTime = 4;
                    item.useAnimation = 20;
                    item.reuseDelay = 30;
                    break;
                case ItemID.StardustCellStaff:
                    item.damage = 65;
                    break;
                case ItemID.Starfury:
                    item.damage = 30;
                    break;
                case ItemID.Terragrim:
                    item.damage = 35;
                    break;
                case ItemID.TheRottedFork:
                    item.damage = 25;
                    break;
                case ItemID.TinShortsword:
                    item.damage = 14;
                    break;
                case ItemID.TitaniumTrident:
                    item.damage = 55;
                    break;
                case ItemID.TragicUmbrella:
                    item.damage = 40;
                    break;
                case ItemID.Trident:
                    item.damage = 19;
                    item.useTime = 25;
                    item.useAnimation = 25;
                    break;
                case ItemID.TungstenShortsword:
                    item.damage = 20;
                    break;
                case ItemID.Umbrella:
                    item.damage = 20;
                    break;
                case ItemID.VampireKnives:
                    item.damage = 35;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.shootSpeed *= 1.5f;
                    break;
                case ItemID.WaterBolt:
                    item.damage = 22;
                    break;
                case ItemID.Zenith:
                    item.damage = 100;
                    break;
            }
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            if (!player.spaceGun) return;
            if (item.type == ItemID.HeatRay || item.type == ItemID.LaserRifle) mult = 0f;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            switch (item.type)
            {
                case ItemID.AdamantiteGlaive:
                    target.AddBuff(BuffID.Electrified, 300);
                    break;
                case ItemID.ChlorophytePartisan:
                    target.AddBuff(BuffID.Venom, 180);
                    break;
                case ItemID.Excalibur:
                    target.AddBuff(ModContent.BuffType<RebukingLight>(), 120);
                    break;
                case ItemID.Gungnir:
                    target.AddBuff(ModContent.BuffType<RebukingLight>(), 120);
                    break;
                case ItemID.FairyQueenMagicItem:
                    target.AddBuff(ModContent.BuffType<RebukingLight>(), 180);
                    break;
                case ItemID.EmpressBlade:
                    target.AddBuff(ModContent.BuffType<RebukingLight>(), 180);
                    break;
                case ItemID.FairyQueenRangedItem:
                    target.AddBuff(ModContent.BuffType<RebukingLight>(), 180);
                    break;
                case ItemID.PiercingStarlight:
                    target.AddBuff(ModContent.BuffType<RebukingLight>(), 180);
                    break;
                case ItemID.RainbowWhip:
                    target.AddBuff(ModContent.BuffType<RebukingLight>(), 180);
                    break;
                case ItemID.RainbowCrystalStaff:
                    target.AddBuff(ModContent.BuffType<PrismaticRend>(), 300);
                    break;
                case ItemID.TitaniumTrident:
                    target.AddBuff(BuffID.OnFire3, 300);
                    break;
            }
        }
    }
}