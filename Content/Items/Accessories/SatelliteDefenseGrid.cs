using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SatelliteDefenseGrid : ModItem
    {
        private const int HEAL_COOLDOWN = 1800;
        private const int EMERGENCY_HEAL_COOLDOWN = 3600;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<SatelliteDefenseGridPlayer>();
            modPlayer.hasSatelliteGrid = true;

            // Provide a defense boost.
            player.statDefense += 10;

            // Ankh Shield immunities
            SetAnkhShieldImmunities(player);

            // Handle health regeneration system
            HandleRegenSystem(player, modPlayer);

            // Handle emergency healing if health is low
            HandleEmergencyHealing(player, modPlayer);

            // The dash ability is now handled in the player class
        }

        private void SetAnkhShieldImmunities(Player player)
        {
            // Set immunities for various debuffs
            int[] immuneBuffs = {
                BuffID.Poisoned, BuffID.Confused, BuffID.CursedInferno, BuffID.Weak,
                BuffID.Silenced, BuffID.BrokenArmor, BuffID.Bleeding, BuffID.Slow,
                BuffID.Electrified
            };

            foreach (int buff in immuneBuffs)
            {
                player.buffImmune[buff] = true;
            }

            player.noKnockback = true;
            player.fireWalk = true;
        }

        private void HandleRegenSystem(Player player, SatelliteDefenseGridPlayer modPlayer)
        {
            if (modPlayer.regenActive)
            {
                player.lifeRegen += 25;
                modPlayer.regenTimer--;
                if (modPlayer.regenTimer <= 0)
                {
                    modPlayer.regenActive = false;
                    modPlayer.healCooldown = HEAL_COOLDOWN;
                }
            }
        }

        private void HandleEmergencyHealing(Player player, SatelliteDefenseGridPlayer modPlayer)
        {
            if ((float)player.statLife / player.statLifeMax2 <= 0.2f && modPlayer.emergencyHealCooldown <= 0)
            {
                player.Heal(200);
                ClearDebuffs(player);
                modPlayer.emergencyHealCooldown = EMERGENCY_HEAL_COOLDOWN;
            }
        }

        private void ClearDebuffs(Player player)
        {
            // Clear player from various debuffs
            int[] debuffsToClear = {
                BuffID.Poisoned, BuffID.OnFire, BuffID.Bleeding, BuffID.Venom,
                BuffID.CursedInferno, BuffID.OnFire3, BuffID.Electrified
            };

            // Clear custom mod-related buffs
            int[] modBuffsToClear = {
                ModContent.BuffType<Demonfire>(),
                ModContent.BuffType<Corrupted>(),
                ModContent.BuffType<PrismaticRend>(),
                ModContent.BuffType<ElementsHarmony>(),
                ModContent.BuffType<GoblinsCurse>(),
                ModContent.BuffType<LunarReap>(),
                ModContent.BuffType<DeadlyCorrupt>(),
                ModContent.BuffType<Sunfire>(),
                ModContent.BuffType<LordsVenom>(),
                ModContent.BuffType<RebukingLight>(),
                ModContent.BuffType<Subjugated>(),
                ModContent.BuffType<OceanPressure>(),
                ModContent.BuffType<Drowning>(),
            };

            foreach (int debuff in debuffsToClear)
            {
                player.ClearBuff(debuff);
            }

            foreach (int modBuff in modBuffsToClear)
            {
                player.ClearBuff(modBuff);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Firewall>())
                .AddIngredient(ModContent.ItemType<NullsDatabase>())
                .AddIngredient(ModContent.ItemType<CyberSphere>())
                .AddIngredient(ItemID.LunarBar, 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}



