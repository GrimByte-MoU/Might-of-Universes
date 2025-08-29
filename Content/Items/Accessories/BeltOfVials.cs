using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class BeltOfVials : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BeltOfVialsPlayer>().hasBeltOfVials = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<VialOfCorruption>(), 1)
                .AddIngredient(ModContent.ItemType<PrismaticVial>(), 1)
                .AddIngredient(ModContent.ItemType<TrappedDemonFire>(), 1)
                .AddIngredient(ModContent.ItemType<LunaticCloth>(), 3)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class BeltOfVialsPlayer : ModPlayer
    {
        public bool hasBeltOfVials = false;
        
        // Track the last weapon used to hit an enemy
        private bool lastHitWasReaper = false;
        
        public override void ResetEffects()
        {
            hasBeltOfVials = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Check if it's a reaper weapon
            lastHitWasReaper = IsReaperWeapon(item);
            
            if (hasBeltOfVials && lastHitWasReaper)
            {
                ApplyDebuffs(target);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Check if it's a reaper projectile
            if (hasBeltOfVials && IsReaperProjectile(proj))
            {
                ApplyDebuffs(target);
            }
        }

        private bool IsReaperWeapon(Item item)
        {
            // Check if the item has a custom damage type (not one of the vanilla types)
            if (item.DamageType != DamageClass.Generic && 
                item.DamageType != DamageClass.Melee && 
                item.DamageType != DamageClass.Ranged && 
                item.DamageType != DamageClass.Magic && 
                item.DamageType != DamageClass.Summon && 
                item.DamageType != DamageClass.Throwing)
            {
                // It's a custom damage type, now check if the player's reaper stats are active
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
            // Apply all three debuffs with different durations
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);  // 3 seconds
            target.AddBuff(ModContent.BuffType<Demonfire>(), 300);      // 5 seconds
            target.AddBuff(ModContent.BuffType<Corrupted>(), 600);      // 10 seconds
            
            // Create a much more brief visual effect
            // Reduce the number of dust particles and make them fade quickly
            for (int i = 0; i < 3; i++) // Reduced from 15 to just 3 particles
            {
                // Create a single dust effect per iteration, randomly choosing the type
                int dustType = Main.rand.Next(new int[] {
                    DustID.RainbowTorch, // Prismatic
                    DustID.Torch,        // Demonfire
                    DustID.PurpleTorch   // Corruption
                });
                
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    dustType,
                    Main.rand.NextFloat(-0.5f, 0.5f), // Greatly reduced velocity
                    Main.rand.NextFloat(-0.5f, 0.5f),
                    100, // Higher alpha = more transparent
                    default,
                    Main.rand.NextFloat(0.5f, 0.8f) // Smaller size
                );
                
                // Make the dust fade very quickly
                dust.noGravity = true;
                dust.fadeIn = 0.05f; // Very quick fade in
                dust.velocity *= 0.3f; // Slow down even more
                
                // Make it disappear faster
                dust.active = true;
                dust.scale *= 0.6f; // Even smaller
            }
        }
    }
}

