using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Rarities;
using System;

namespace MightofUniverses.Content.Items.Accessories
{
    public class WarcrimeSash : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<TacticianRarity>(); // Using your custom Tactician rarity
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<WarcrimeSashPlayer>().hasWarcrimeSash = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BeltOfVials>(), 1)
                .AddIngredient(ModContent.ItemType<VialofLordsVenom>(), 1)
                .AddIngredient(ModContent.ItemType<LiquidTerra>(), 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class WarcrimeSashPlayer : ModPlayer
    {
        public bool hasWarcrimeSash = false;
        
        // Track the last weapon used to hit an enemy
        private Item lastWeaponUsed;
        private bool lastHitWasReaper = false;
        
        public override void ResetEffects()
        {
            hasWarcrimeSash = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Store the weapon used and check if it's a reaper weapon
            lastWeaponUsed = item;
            lastHitWasReaper = IsReaperWeapon(item);
            
            if (hasWarcrimeSash && lastHitWasReaper)
            {
                ApplyDebuffs(target);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // For projectiles, we need to check if the projectile is from a reaper weapon
            if (hasWarcrimeSash && IsReaperProjectile(proj))
            {
                ApplyDebuffs(target);
            }
        }

        private bool IsReaperWeapon(Item item)
        {
            // Check if the item has the ReaperDamageClass
            // This is the most reliable way to check if it's a reaper weapon
            
            // First check: See if the item has a custom damage type that might be Reaper
            if (item.DamageType != DamageClass.Generic && 
                item.DamageType != DamageClass.Melee && 
                item.DamageType != DamageClass.Ranged && 
                item.DamageType != DamageClass.Magic && 
                item.DamageType != DamageClass.Summon && 
                item.DamageType != DamageClass.Throwing)
            {
                // It's a custom damage type, now check if the player's reaper multiplier is active
                var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                
                // If the player just consumed souls, it's likely a reaper weapon
                if (reaperPlayer.justConsumedSouls)
                {
                    return true;
                }
                
                // If the player has reaper armor, it's more likely to be a reaper weapon
                if (reaperPlayer.hasReaperArmor)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        private bool IsReaperProjectile(Projectile proj)
        {
            // Check if the projectile is from a Reaper weapon
            var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
            
            // If the player just consumed souls, it's likely a reaper projectile
            if (reaperPlayer.justConsumedSouls && proj.owner == Player.whoAmI)
            {
                return true;
            }
            
            // If the player has reaper armor and the projectile is from this player
            if (reaperPlayer.hasReaperArmor && proj.owner == Player.whoAmI)
            {
                // Additional check: if the projectile was created recently after using a reaper weapon
                if (lastHitWasReaper && proj.timeLeft > proj.MaxUpdates - 10)
                {
                    return true;
                }
            }
            
            return false;
        }

        private void ApplyDebuffs(NPC target)
        {
            // Apply all debuffs with specified durations
            target.AddBuff(ModContent.BuffType<LordsVenom>(), 180);       // 3 seconds
            target.AddBuff(ModContent.BuffType<Sunfire>(), 240);          // 4 seconds
            target.AddBuff(ModContent.BuffType<ElementsHarmony>(), 300);  // 5 seconds
            target.AddBuff(ModContent.BuffType<DeadlyCorrupt>(), 300);    // 5 seconds
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 360);    // 6 seconds
            
            // Create a much more brief visual effect
            // Reduce the number of dust particles and make them fade quickly
            for (int i = 0; i < 5; i++) // Reduced from 20 to 5 particles
            {
                // Create a single combined dust effect instead of multiple types
                int dustType = Main.rand.Next(new int[] {
                    DustID.Poisoned,    // Lords Venom (green)
                    DustID.Torch,       // Sunfire (orange)
                    DustID.PurpleTorch, // DeadlyCorrupt (purple)
                    DustID.RainbowTorch // Prismatic (rainbow)
                });
                
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    dustType,
                    Main.rand.NextFloat(-1f, 1f), // Reduced velocity
                    Main.rand.NextFloat(-1f, 1f),
                    100, // Higher alpha = more transparent
                    default,
                    Main.rand.NextFloat(0.7f, 1.0f) // Smaller size
                );
                
                // Make the dust fade very quickly
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.velocity *= 0.5f;
                
                // Set a very short lifetime
                if (dust.customData == null)
                {
                    dust.customData = 1;
                }
                
                // Make it disappear faster
                dust.active = true;
                dust.scale *= 0.8f;
            }
        }
    }
}

