using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Systems
{
    public class WingBonusesGlobalItem : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item is null || player is null) return;

            var p = player.GetModPlayer<WingBonusPlayer>();

            void AddLife(int amount) => player.statLifeMax2 += amount;
            void AddMana(int amount) => player.statManaMax2 += amount;

            bool isWingish = item.wingSlot >= 0
                             || item.type == WingIds.Jetpack
                             || item.type == WingIds.Hoverboard
                             || item.type == WingIds.CelestialStarboard
                             || item.type == WingIds.NebulaMantle
                             || item.type == WingIds.VortexBooster;

            if (!isWingish)
                return;

            int t = item.type;

             if (t == WingIds.AngelWings)
            {
                AddLife(20);
                player.lifeRegen += 2;
            }
            else if (t == WingIds.DemonWings)
            {
                AddLife(20);
                player.GetDamage(DamageClass.Generic) += 0.05f;
            }
            else if (t == WingIds.LeafWings)
            {
                player.maxMinions += 1;
                player.statDefense += 4;
            }
            else if (t == WingIds.BetsyWings)
            {
                player.maxMinions += 2;
                player.maxTurrets += 2;
                player.GetDamage(DamageClass.Summon) -= 0.10f;
            }
            else if (t == WingIds.Jetpack)
            {
                player.statDefense += 5;
                player.jumpSpeedBoost += 0.60f;
            }
            else if (t == WingIds.FairyWings)
            {
                AddLife(50);
            }
            else if (t == WingIds.FinWings)
            {
                player.ignoreWater = true;
                player.accFlipper = true;
                if (player.wet && !player.lavaWet)
                    player.moveSpeed += 0.50f;
            }
            else if (t == WingIds.HarpyWings)
            {
                player.GetDamage(DamageClass.Generic) += 0.07f;
            }
            else if (t == WingIds.FrozenWings)
            {
                Immune(player, BuffID.Frostburn, BuffID.Frostburn2, BuffID.Chilled, BuffID.Frozen);
            }
            else if (t == WingIds.FlameWings)
            {
                Immune(player, BuffID.OnFire3, BuffID.CursedInferno, BuffID.Burning);
            }
            else if (t == WingIds.GhostWings)
            {
                AddMana(50);
                player.manaCost -= 0.10f;
            }
            else if (t == WingIds.BeeWings)
            {
                player.GetDamage(DamageClass.Magic) += 0.10f;
                Immune(player, BuffID.Poisoned, BuffID.Venom);
            }
            else if (t == WingIds.ButterflyWings)
            {
                AddMana(100);
            }
            else if (t == WingIds.BatWings)
            {
                player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) += 0.10f;
                player.GetCritChance(ModContent.GetInstance<ReaperDamageClass>()) += 5;
            }
            else if (t == WingIds.Hoverboard)
            {
                p.AmmoConserveChance = Math.Max(p.AmmoConserveChance, 0.15f);
                player.GetDamage(DamageClass.Ranged) += 0.10f;
            }
            else if (t == WingIds.BoneWings)
            {
                player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) += 0.10f;
            }
            else if (t == WingIds.MothronWings)
            {
                if (Main.eclipse)
                    p.MothronEclipseBoost = true;
            }
            else if (t == WingIds.BeetleWings)
            {
                player.statDefense += 8;
                player.lifeRegen += 5;
            }
            else if (t == WingIds.SpookyWings)
            {
                player.maxMinions += 1;
                player.GetDamage(DamageClass.Summon) += 0.10f;
            }
            else if (t == WingIds.TatteredFairyWings)
            {
                if (Main.eclipse || !Main.dayTime)
                    AddLife(30);
                if (Main.eclipse || Main.dayTime)
                    player.GetDamage(DamageClass.Generic) += 0.10f;
            }
            else if (t == WingIds.SteampunkWings)
            {
            }
            else if (t == WingIds.FishronWings)
            {
                if (player.wet)
                    p.FishronInfiniteWaterFlight = true;
            }
            else if (t == WingIds.SolarWings)
            {
                player.GetDamage(DamageClass.Melee) += 0.15f;
            }
            else if (t == WingIds.StardustWings)
            {
                player.maxMinions += 2;
            }
            else if (t == WingIds.CelestialStarboard)
            {
                player.GetDamage(ModContent.GetInstance<PacifistDamageClass>()) += 0.80f;
            }
            else if (t == WingIds.NebulaMantle)
            {
                player.GetDamage(DamageClass.Magic) += 0.15f;
            }
            else if (t == WingIds.VortexBooster)
            {
                player.GetDamage(DamageClass.Ranged) += 0.15f;
            }
            else if (t == WingIds.FestiveWings)
            {
                player.GetDamage(ModContent.GetInstance<PacifistDamageClass>()) += 0.50f;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item == null) return;

            bool isWingish = item.wingSlot >= 0
                             || item.type == WingIds.Jetpack
                             || item.type == WingIds.Hoverboard
                             || item.type == WingIds.CelestialStarboard
                             || item.type == WingIds.NebulaMantle
                             || item.type == WingIds.VortexBooster;

            if (!isWingish)
                return;

            string bonus = GetBonusText(item.type);

            if (!string.IsNullOrEmpty(bonus))
            {
                string[] lines = bonus.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    TooltipLine tooltipLine = new TooltipLine(Mod, "WingBonus" + i, lines[i]);
                    tooltips.Add(tooltipLine);
                }
            }
        }

        private string GetBonusText(int itemType)
        {
             if (itemType == WingIds.AngelWings)
                return "+20 max life\n+2 life regeneration";
            else if (itemType == WingIds.DemonWings)
                return "+20 max life\n+5% damage";
            else if (itemType == WingIds.LeafWings)
                return "+1 max minion\n+4 defense";
            else if (itemType == WingIds.BetsyWings)
                return "+2 max minions\n+2 max sentries\n-10% summon damage";
            else if (itemType == WingIds.Jetpack)
                return "+5 defense\n+60% jump speed";
            else if (itemType == WingIds.FairyWings)
                return "+50 max life";
            else if (itemType == WingIds.FinWings)
                return "Grants water walking and swimming\n+50% movement speed in water";
            else if (itemType == WingIds.HarpyWings)
                return "+7% damage";
            else if (itemType == WingIds.FrozenWings)
                return "Immunity to Frostburn, Chilled, and Frozen";
            else if (itemType == WingIds.FlameWings)
                return "Immunity to On Fire, Cursed Inferno, and Burning";
            else if (itemType == WingIds.GhostWings)
                return "+50 max mana\n-10% mana cost";
            else if (itemType == WingIds.BeeWings)
                return "+10% magic damage\nImmunity to Poisoned and Venom";
            else if (itemType == WingIds.ButterflyWings)
                return "+100 max mana";
            else if (itemType == WingIds.BatWings)
                return "+10% reaper damage\n+5% reaper critical strike chance";
            else if (itemType == WingIds.Hoverboard)
                return "15% chance not to consume ammo\n+10% ranged damage";
            else if (itemType == WingIds.BoneWings)
                return "+10% reaper damage";
            else if (itemType == WingIds.MothronWings)
                return "Infinite flight time during Solar Eclipse";
            else if (itemType == WingIds.BeetleWings)
                return "+8 defense\n+5 life regeneration";
            else if (itemType == WingIds.SpookyWings)
                return "+1 max minion\n+10% summon damage";
            else if (itemType == WingIds.TatteredFairyWings)
                return "+30 max life at night or during Eclipse\n+10% damage during day or Eclipse";
            else if (itemType == WingIds.FishronWings)
                return "Infinite flight time in water";
            else if (itemType == WingIds.SolarWings)
                return "+15% melee damage";
            else if (itemType == WingIds.StardustWings)
                return "+2 max minions";
            else if (itemType == WingIds.CelestialStarboard)
                return "+80% nonweapon damage";
            else if (itemType == WingIds.NebulaMantle)
                return "+15% magic damage";
            else if (itemType == WingIds.VortexBooster)
                return "+15% ranged damage";
            else if (itemType == WingIds.FestiveWings)
                return "+50% nonweapon damage";

            return string.Empty;
        }

        private static void Immune(Player player, params int[] buffIds)
        {
            foreach (var id in buffIds)
            {
                if (id >= 0 && id < BuffID.Count)
                    player.buffImmune[id] = true;
            }
        }

        private static void TryImmune(Player player, string modName, string buffInternalName)
        {
            if (ModLoader.TryGetMod(modName, out var mod) && mod.TryFind(buffInternalName, out ModBuff buff))
            {
                player.buffImmune[buff.Type] = true;
            }
        }
    }

    public class WingBonusPlayer : ModPlayer
    {
        public float AmmoConserveChance;
        public bool MothronEclipseBoost;
        public bool FishronInfiniteWaterFlight;

        public override void ResetEffects()
        {
            AmmoConserveChance = 0f;
            MothronEclipseBoost = false;
            FishronInfiniteWaterFlight = false;
        }

        public override void PostUpdate()
        {
            if (FishronInfiniteWaterFlight && Player.wet)
            {
                Player.wingTime = Player.wingTimeMax;
            }

            if (MothronEclipseBoost && Main.eclipse && Player.controlJump && Player.wingTime > 0)
            {
                Player.wingTime = Math.Min(Player.wingTimeMax, Player.wingTime + 1);
            }
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            if (AmmoConserveChance > 0f && weapon != null && weapon.useAmmo > 0)
            {
                if (Main.rand.NextFloat() < AmmoConserveChance)
                    return false;
            }
            return base.CanConsumeAmmo(weapon, ammo);
        }
    }

    internal static class WingIds
    {
        public static readonly int AngelWings         = ItemID.AngelWings;
        public static readonly int DemonWings         = ItemID.DemonWings;
        public static readonly int LeafWings          = ItemID.LeafWings;
        public static readonly int BetsyWings         = ItemID.BetsyWings;
        public static readonly int FairyWings         = ItemID.FairyWings;
        public static readonly int FinWings           = ItemID.FinWings;
        public static readonly int HarpyWings         = ItemID.HarpyWings;
        public static readonly int FrozenWings        = ItemID.FrozenWings;
        public static readonly int FlameWings         = ItemID.FlameWings;
        public static readonly int GhostWings         = ItemID.GhostWings;
        public static readonly int BeeWings           = ItemID.BeeWings;
        public static readonly int ButterflyWings     = ItemID.ButterflyWings;
        public static readonly int BatWings           = ItemID.BatWings;
        public static readonly int BoneWings          = ItemID.BoneWings;
        public static readonly int MothronWings       = ItemID.MothronWings;
        public static readonly int BeetleWings        = ItemID.BeetleWings;
        public static readonly int SpookyWings        = ItemID.SpookyWings;
        public static readonly int TatteredFairyWings = ItemID.TatteredFairyWings;
        public static readonly int SteampunkWings     = ItemID.SteampunkWings;
        public static readonly int FishronWings       = ItemID.FishronWings;
        public static readonly int SolarWings         = ItemID.WingsSolar;
        public static readonly int StardustWings      = ItemID.WingsStardust;
        public static readonly int FestiveWings       = ItemID.FestiveWings;

        public static readonly int CelestialStarboard = ItemID.LongRainbowTrailWings;
        public static readonly int NebulaMantle       = ItemID.WingsNebula;
        public static readonly int VortexBooster      = ItemID.WingsVortex;

        public static readonly int Jetpack            = ItemID.Jetpack;
        public static readonly int Hoverboard         = ItemID.Hoverboard;
    }
}