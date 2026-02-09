using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class HellstakeProjectile : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.Torch, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
                dust.scale = 1.2f + Main.rand.NextFloat() * 0.5f;
                dust.velocity *= 0.3f;
            }
            
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 180);
            target.AddBuff(ModContent.BuffType<Demonfire>(), 180);
            CreateExplosion();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            CreateExplosion();
            return true;
        }
        
        private void CreateExplosion()
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            
            for (int i = 0; i < 50; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, speed * 5f, Scale: 2f);
                dust.noGravity = true;
                
                if (i % 2 == 0)
                {
                    dust.type = DustID.Flare;
                }
            }

            float explosionRadius = 100f;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (distance < explosionRadius)
                    {
                        npc.AddBuff(BuffID.OnFire, 60);
                        npc.AddBuff(ModContent.BuffType<Demonfire>(), 60);
                        int explosionDamage = (int)(Projectile.damage * 0.75f * (1f - distance / explosionRadius));

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
