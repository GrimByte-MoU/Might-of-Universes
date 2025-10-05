using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Accessories
{
    public class MoodyStorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName and Tooltip are automatically set from .hjson files in 1.4.4
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MoodyStormPlayer>().hasMoodyStorm = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ModContent.ItemType<LivingStorm>(), 1)
                .AddTile(TileID.SkyMill)
                .Register();
        }
    }

    public class MoodyStormPlayer : ModPlayer
    {
        public bool hasMoodyStorm = false;
        private int stormTimer = 0;
        private const int STORM_INTERVAL = 45; // 3/4 seconds
        private const float STORM_RADIUS = 15f * 16f; // 10 tiles in pixels
        private const int STORM_DAMAGE = 45; // Base damage

        public override void ResetEffects()
        {
            hasMoodyStorm = false;
        }

        public override void PostUpdate()
        {
            if (!hasMoodyStorm) return;

            // Draw the circle outline
            const int NUM_POINTS = 60; // Number of points in the circle
            for (int i = 0; i < NUM_POINTS; i++)
            {
                float angle = (float)(i * (2 * Math.PI) / NUM_POINTS);
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle) * STORM_RADIUS,
                    (float)Math.Sin(angle) * STORM_RADIUS
                );
                Vector2 dustPos = Player.Center + offset;
                
                // Create bright yellow dust at each point
                Dust dust = Dust.NewDustPerfect(
                    dustPos,
                    DustID.YellowTorch,
                    Vector2.Zero,
                    0,
                    Color.Yellow,
                    1f
                );
                dust.noGravity = true;
                dust.noLight = false;
            }

            // Handle damage every 60 ticks (1 second)
            stormTimer++;
            if (stormTimer >= STORM_INTERVAL)
            {
                stormTimer = 0;
                
                // Find all NPCs in range
                List<NPC> affectedNPCs = new List<NPC>();
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage && 
                        Vector2.Distance(npc.Center, Player.Center) <= STORM_RADIUS)
                    {
                        affectedNPCs.Add(npc);
                    }
                }

                // Apply damage and effects to each NPC
                if (affectedNPCs.Count > 0)
                {
                    var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                    
                    foreach (NPC npc in affectedNPCs)
                    {
                        // Apply damage
                        if (Main.myPlayer == Player.whoAmI)
                        {
                            npc.SimpleStrikeNPC(STORM_DAMAGE, 0);
                            
                            // Apply electrified debuff for 1 second
                            npc.AddBuff(BuffID.Electrified, 120);
                        }
                        
                        // Add soul energy (1 per enemy hit)
                        reaperPlayer.AddSoulEnergy(2f, npc.Center);
                        
                        // Create lightning effect at enemy position
                        CreateLightningEffect(npc.Center);
                    }
                    
                    // Create storm sound effect
                    if (affectedNPCs.Count > 0 && Main.rand.NextBool(2))
                    {
                        SoundEngine.PlaySound(SoundID.Item122, Player.Center); // Thunder sound
                    }
                }
                
                // Create additional storm visuals
                CreateStormVisuals();
            }
        }
        
        private void CreateLightningEffect(Vector2 position)
        {
            // Create electric dust at the target
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                Dust dust = Dust.NewDustPerfect(
                    position,
                    DustID.Electric,
                    velocity,
                    0,
                    Color.White,
                    Main.rand.NextFloat(1f, 1.5f)
                );
                dust.noGravity = true;
            }
            
            // Occasionally create a lightning bolt from player to target
            if (Main.rand.NextBool(2))
            {
                Vector2 direction = position - Player.Center;
                float distance = direction.Length();
                direction.Normalize();
                
                // Create a jagged lightning path
                Vector2 currentPos = Player.Center;
                int segments = (int)(distance / 30f);
                for (int i = 0; i < segments; i++)
                {
                    Vector2 nextPos = Player.Center + direction * distance * ((i + 1f) / segments);
                    // Add some randomness to the path
                    nextPos += Main.rand.NextVector2Circular(10f, 10f);
                    
                    // Draw dust between points
                    Vector2 dustDir = nextPos - currentPos;
                    float dustDist = dustDir.Length();
                    dustDir.Normalize();
                    
                    for (float j = 0; j < dustDist; j += 5f)
                    {
                        Vector2 dustPos = currentPos + dustDir * j;
                        Dust dust = Dust.NewDustPerfect(
                            dustPos,
                            DustID.Electric,
                            Vector2.Zero,
                            0,
                            Color.White,
                            1f
                        );
                        dust.noGravity = true;
                    }
                    
                    currentPos = nextPos;
                }
            }
        }
        
        private void CreateStormVisuals()
        {
            // Create cloud and rain effects within the storm area
            for (int i = 0; i < 15; i++)
            {
                Vector2 position = Player.Center + Main.rand.NextVector2Circular(STORM_RADIUS * 0.8f, STORM_RADIUS * 0.8f);
                
                // Cloud dust
                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustPerfect(
                        position,
                        DustID.Cloud,
                        Main.rand.NextVector2Circular(1f, 1f),
                        0,
                        Color.White,
                        Main.rand.NextFloat(1f, 1.5f)
                    );
                    dust.noGravity = true;
                }
                
                // Rain dust
                Dust rainDust = Dust.NewDustPerfect(
                    position,
                    DustID.Rain,
                    new Vector2(0f, 5f),
                    0,
                    Color.LightBlue,
                    1f
                );
                rainDust.noGravity = false;
            }
        }
    }
}