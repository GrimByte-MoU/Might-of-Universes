using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SollutumStakeProjectile : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sollutum Stake");
        }

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 1; 
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 6; // Pierce 6 enemies
            Projectile.timeLeft = 600;
            Projectile.light = 0.8f; // Brighter light
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            // Create a trail of solar particles
            if (Main.rand.NextBool(2)) // More frequent dust
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.SolarFlare, 0f, 0f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                
                // Add some Torch dust for extra effect
                if (Main.rand.NextBool(3))
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 
                        DustID.Torch, 0f, 0f, 100, default, 1f);
                    dust2.noGravity = true;
                    dust2.velocity *= 0.2f;
                }
            }
            
            // Rotate the projectile based on velocity
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            
            target.AddBuff(BuffID.Daybreak, 180);
            target.AddBuff(ModContent.BuffType<Sunfire>(), 180);
            
            // Create explosion for each enemy hit
            CreateExplosion(target.Center);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Create explosion when hitting a tile
            CreateExplosion(Projectile.Center);
            return true; // Destroy the projectile
        }
        
        private void CreateExplosion(Vector2 position)
        {
            // Play explosion sound
            SoundEngine.PlaySound(SoundID.Item14, position);
            
            // Create explosion visual effect
            for (int i = 0; i < 70; i++) // More particles
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust dust;
                
                if (i % 3 == 0)
                {
                    dust = Dust.NewDustPerfect(position, DustID.SolarFlare, speed * 7f, Scale: 2.5f);
                }
                else if (i % 3 == 1)
                {
                    dust = Dust.NewDustPerfect(position, DustID.Torch, speed * 6f, Scale: 2f);
                }
                else
                {
                    dust = Dust.NewDustPerfect(position, DustID.Flare, speed * 5f, Scale: 1.8f);
                }
                
                dust.noGravity = true;
            }
            
            // Damage enemies in explosion radius
            float explosionRadius = 150f; // Larger radius
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                    float distance = Vector2.Distance(npc.Center, position);
                    if (distance < explosionRadius)
                    {
                        // Apply Sunfire debuff to enemies hit by explosion
                        npc.AddBuff(ModContent.BuffType<Sunfire>(), 180);
                        
                        // Calculate damage based on distance from explosion center
                        int explosionDamage = (int)(Projectile.damage * 1.5f * (1f - distance / explosionRadius));
                        
                        // Deal damage
                        if (explosionDamage > 0)
                        {
                            npc.SimpleStrikeNPC(explosionDamage, 0);
                        }
                    }
                }
            }
        }
    }
}
