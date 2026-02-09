using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Accessories
{
    public class LivingStorm : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<LivingStormPlayer>().hasLivingStorm = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RainCloud, 25)
                .AddIngredient(ItemID.Cloud, 15)
                .AddTile(TileID.SkyMill)
                .Register();
        }
    }

    public class LivingStormPlayer : ModPlayer
    {
        public bool hasLivingStorm = false;
        private int stormTimer = 0;
        private const int STORM_INTERVAL = 60;
        private const float STORM_RADIUS = 10f * 16f;
        private const int STORM_DAMAGE = 15;

        public override void ResetEffects()
        {
            hasLivingStorm = false;
        }

        public override void PostUpdate()
        {
            if (!hasLivingStorm) return;
            const int NUM_POINTS = 60;
            for (int i = 0; i < NUM_POINTS; i++)
            {
                float angle = (float)(i * (2 * Math.PI) / NUM_POINTS);
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle) * STORM_RADIUS,
                    (float)Math.Sin(angle) * STORM_RADIUS
                );
                Vector2 dustPos = Player.Center + offset;
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

            stormTimer++;
            if (stormTimer >= STORM_INTERVAL)
            {
                stormTimer = 0;
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

                if (affectedNPCs.Count > 0)
                {
                    var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                    
                    foreach (NPC npc in affectedNPCs)
                    {
                        if (Main.myPlayer == Player.whoAmI)
                        {
                            npc.SimpleStrikeNPC(STORM_DAMAGE, 0);
                            npc.AddBuff(BuffID.Electrified, 60);
                        }
        
                        reaperPlayer.AddSoulEnergy(1f, npc.Center);
                        CreateLightningEffect(npc.Center);
                    }

                    if (affectedNPCs.Count > 0 && Main.rand.NextBool(3))
                    {
                        SoundEngine.PlaySound(SoundID.Item122, Player.Center); // Thunder sound
                    }
                }

                CreateStormVisuals();
            }
        }
        
        private void CreateLightningEffect(Vector2 position)
        {
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

            if (Main.rand.NextBool(3))
            {
                Vector2 direction = position - Player.Center;
                float distance = direction.Length();
                direction.Normalize();
                Vector2 currentPos = Player.Center;
                int segments = (int)(distance / 30f);
                for (int i = 0; i < segments; i++)
                {
                    Vector2 nextPos = Player.Center + direction * distance * ((i + 1f) / segments);
                    nextPos += Main.rand.NextVector2Circular(10f, 10f);
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
            for (int i = 0; i < 15; i++)
            {
                Vector2 position = Player.Center + Main.rand.NextVector2Circular(STORM_RADIUS * 0.8f, STORM_RADIUS * 0.8f);

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
