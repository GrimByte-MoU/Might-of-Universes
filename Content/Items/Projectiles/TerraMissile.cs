using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna. Framework;
using MightofUniverses.Content.Items. Buffs;
using MightofUniverses.Common;
using System;

namespace MightofUniverses.Content. Items.Projectiles
{
    public class TerraMissile : MoUProjectile
    {

        private int homingDelay = 30;

        public override void SafeSetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            homingDelay--;

            if (homingDelay <= 0)
            {
                NPC target = FindClosestEnemy(800f);
                if (target != null)
                {
                    Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 18f, 0.06f);
                }
            }

            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID. TerraBlade, 0, 0, 100, Color. LimeGreen, 2.0f);
                dust. noGravity = true;
                dust.velocity = -Projectile.velocity * 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 1.0f, 0.5f);
        }

        private NPC FindClosestEnemy(float maxDistance)
        {
            NPC closestNPC = null;
            float closestDist = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc. friendly && !npc.dontTakeDamage)
                {
                    float distance = Vector2.Distance(Projectile. Center, npc.Center);
                    if (distance < closestDist)
                    {
                        closestDist = distance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.LimeGreen;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.5f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
            }

            Terraria.Audio.SoundEngine. PlaySound(SoundID.Item14, Projectile.Center);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 1.5f);
                dust.noGravity = true;
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID. Item14, Projectile.Center);
            return true;
        }
    }
}