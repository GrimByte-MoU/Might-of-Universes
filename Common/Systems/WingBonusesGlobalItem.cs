using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Systems
{
    public class WingBonusesGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => false;

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

            if (t == WingIds.FledglingWings)
            {
                player.moveSpeed += 0.05f;
            }
            else if (t == WingIds.AngelWings)
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
            else if (t == WingIds.EmpressWings)
            {
                player.moveSpeed += 0.20f;
                TryImmune(player, "MightofUniverses", "PrismaticRend");
                TryImmune(player, "MightofUniverses", "PrismaticRendDebuff");
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
            else if (t == WingIds.SpectreWings)
            {
                player.manaRegenBonus += 2;
                player.GetCritChance(DamageClass.Magic) += 5;
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
        public static readonly int FledglingWings     = Find("FledglingWings");
        public static readonly int AngelWings         = Find("AngelWings");
        public static readonly int DemonWings         = Find("DemonWings");
        public static readonly int LeafWings          = Find("LeafWings");
        public static readonly int BetsyWings         = Find("BetsyWings");
        public static readonly int EmpressWings       = Find("EmpressWings");
        public static readonly int FairyWings         = Find("FairyWings");
        public static readonly int FinWings           = Find("FinWings");
        public static readonly int HarpyWings         = Find("HarpyWings");
        public static readonly int FrozenWings        = Find("FrozenWings");
        public static readonly int FlameWings         = Find("FlameWings");
        public static readonly int GhostWings         = Find("GhostWings");
        public static readonly int BeeWings           = Find("BeeWings");
        public static readonly int ButterflyWings     = Find("ButterflyWings");
        public static readonly int BatWings           = Find("BatWings");
        public static readonly int BoneWings          = Find("BoneWings");
        public static readonly int MothronWings       = Find("MothronWings");
        public static readonly int SpectreWings       = Find("SpectreWings");
        public static readonly int BeetleWings        = Find("BeetleWings");
        public static readonly int SpookyWings        = Find("SpookyWings");
        public static readonly int TatteredFairyWings = Find("TatteredFairyWings");
        public static readonly int SteampunkWings     = Find("SteampunkWings");
        public static readonly int FishronWings       = Find("FishronWings");
        public static readonly int SolarWings         = Find("SolarWings");
        public static readonly int StardustWings      = Find("StardustWings");
        public static readonly int FestiveWings       = Find("FestiveWings");

        public static readonly int CelestialStarboard = Find("CelestialStarboard");
        public static readonly int NebulaMantle       = Find("NebulaMantle");
        public static readonly int VortexBooster      = Find("VortexBooster");

        public static readonly int Jetpack            = Find("Jetpack");
        public static readonly int Hoverboard         = Find("Hoverboard");

        private static int Find(string name)
        {
            return ItemID.Search.TryGetId(name, out int id) ? id : -1;
        }
    }
}