using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace MightofUniverses.Content.Items.Accessories
{
    public class WraithwalkerEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName and Tooltip are set in localization files
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Increase Reaper and Ranged damage
            player.GetDamage(DamageClass.Ranged) += 0.12f;
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.reaperDamageMultiplier += 0.12f;

            // Increase Reaper and Ranged crit chance
            player.GetCritChance(DamageClass.Ranged) += 10;
            reaperPlayer.reaperCritChance += 10;

            // Apply the Wraithwalker effects
            player.GetModPlayer<WraithwalkerEmblemPlayer>().hasWraithwalkerEmblem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentVortex, 15)
                .AddIngredient(ModContent.ItemType<ReaperEmblem>(), 1)
                .AddIngredient(ItemID.DestroyerEmblem, 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class WraithwalkerEmblemPlayer : ModPlayer
    {
        public bool hasWraithwalkerEmblem = false;
        private int ammoConsumptionTimer = 0;

        public override void ResetEffects()
        {
            hasWraithwalkerEmblem = false;
        }

        public override void PostUpdate()
        {
            if (!hasWraithwalkerEmblem) return;

            // Consume ammo every 2 seconds
            ammoConsumptionTimer++;
            if (ammoConsumptionTimer >= 120)
            {
                ammoConsumptionTimer = 0;
                ConsumeAmmoForSouls();
            }

            // Apply soul-based bonuses to ranged attacks
            var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
            float soulBonus = reaperPlayer.soulEnergy / 10f;
            
            // Damage bonus
            Player.GetDamage(DamageClass.Ranged) += soulBonus / 100f; // 1% per 10 soul energy
            
            // Attack speed bonus
            Player.GetAttackSpeed(DamageClass.Ranged) += soulBonus / 100f; // 1% per 10 soul energy
            
            // Armor penetration bonus
            Player.GetArmorPenetration(DamageClass.Ranged) += (int)(soulBonus); // +1 per 10 soul energy
        }

        private void ConsumeAmmoForSouls()
        {
            // Find ammo in inventory
            Item ammo = null;
            int ammoSlot = -1;
            
            // Check for ammo in ammo slots first
            for (int i = 54; i < 58; i++)
            {
                Item item = Player.inventory[i];
                if (item.ammo != AmmoID.None && item.stack > 0)
                {
                    ammo = item;
                    ammoSlot = i;
                    break;
                }
            }
            
            // If no ammo in ammo slots, check regular inventory
            if (ammo == null)
            {
                for (int i = 0; i < 54; i++)
                {
                    Item item = Player.inventory[i];
                    if (item.ammo != AmmoID.None && item.stack > 0)
                    {
                        ammo = item;
                        ammoSlot = i;
                        break;
                    }
                }
            }
            
            // If ammo found, consume it and gain souls
            if (ammo != null && ammoSlot != -1)
            {
                // Calculate base damage of the ammo
                int ammoDamage = ammo.damage;
                
                // Consume one ammo
                Player.inventory[ammoSlot].stack--;
                if (Player.inventory[ammoSlot].stack <= 0)
                {
                    Player.inventory[ammoSlot].TurnToAir();
                }
                
                // Calculate soul energy to gain (half of ammo damage)
                int soulEnergyGain = Math.Max(1, ammoDamage / 2);
                
                // Add soul energy
                var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                reaperPlayer.AddSoulEnergy(soulEnergyGain, Player.Center);
                
                // Visual effect
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(
                        Player.position,
                        Player.width,
                        Player.height,
                        DustID.Vortex,
                        Main.rand.NextFloat(-2f, 2f),
                        Main.rand.NextFloat(-2f, 2f),
                        0,
                        default,
                        Main.rand.NextFloat(1f, 1.5f)
                    );
                }
                
                // Show text
                if (Main.myPlayer == Player.whoAmI)
                {
                    CombatText.NewText(Player.getRect(), Color.Cyan, "+" + soulEnergyGain + " Soul Energy");
                }
            }
        }
    }
}
