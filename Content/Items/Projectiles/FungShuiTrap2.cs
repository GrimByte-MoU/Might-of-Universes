using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FungShuiTrap2 : MoUProjectile
    {
        // Homing parameters
        private const float SearchRadius = 600f;
        private const float HomingSpeed = 10f;
        private const float HomingInertia = 20f;
        private const float ExplodeDistance = 28f;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.width = 16;
            Projectile.height = 16;
        }

        public override void AI()
        {
            // Acquire nearest valid target
            int targetIndex = -1;
            float bestDistSq = SearchRadius * SearchRadius;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc == null || !npc.active || npc.friendly || npc.life <= 0)
                    continue;
                if (npc.townNPC || npc.type == NPCID.TargetDummy)
                    continue;

                float distSq = Vector2.DistanceSquared(npc.Center, Projectile.Center);
                if (distSq < bestDistSq)
                {
                    bestDistSq = distSq;
                    targetIndex = i;
                }
            }

            if (targetIndex >= 0)
            {
                NPC target = Main.npc[targetIndex];
                Vector2 desired = target.Center - Projectile.Center;
                float dist = desired.Length();
                if (dist > 0.01f)
                {
                    desired.Normalize();
                    desired *= HomingSpeed;
                    Projectile.velocity = (Projectile.velocity * (HomingInertia - 1f) + desired) / HomingInertia;
                }

                // Explode when close enough
                if (Vector2.Distance(Projectile.Center, target.Center) <= ExplodeDistance)
                {
                    Explode();
                    return;
                }
            }
            else
            {
                // Idle: slight drift
                Projectile.velocity *= 0.98f;
            }

            // Visual light
            Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 0.8f);
            Projectile.rotation += 0.02f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
        }

        private void Explode()
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            for (int i = 0; i < 14; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(4f, 4f);
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, vel.X, vel.Y);
                Main.dust[d].noGravity = true;
                Main.dust[d].scale = 1.1f;
            }

            Projectile.netUpdate = true;
            Projectile.Kill(); // Kill() spawns FungShuiSpore2 in original Kill implementation
        }

        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FungShuiSpore2>(), 35, 0, Main.myPlayer);
            }

            for (int i = 0; i < 8; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(2f, 2f);
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, vel.X, vel.Y);
                Main.dust[d].noGravity = true;
                Main.dust[d].scale = 0.9f;
            }
        }
    }
}