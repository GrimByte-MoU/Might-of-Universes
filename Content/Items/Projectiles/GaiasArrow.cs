using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class GaiasArrow : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
            Projectile.aiStyle = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {

            if (Projectile.velocity.Length() > 0.1f)
                Projectile.rotation = Projectile.velocity.ToRotation();


            NPC target = FindClosestTarget(Projectile.Center, 480f);
            if (target != null)
            {
                float accel = 0.18f;
                float maxSpeed = 18f;

                Vector2 desired = Projectile.DirectionTo(target.Center) * maxSpeed;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, accel);
            }
            if (target == null)
                Projectile.velocity *= 0.995f;

            if (++Projectile.localAI[1] >= 6f)
            {
                Projectile.localAI[1] = 0f;
                if (Projectile.velocity.Length() < 6f)
                    Shatter();
            }
            Lighting.AddLight(Projectile.Center, 0f, 2f, 0f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Shatter();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 3 seconds of Terra's Rend
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);
            Shatter();
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.localAI[0] == 0f)
                Shatter();
        }

        private NPC FindClosestTarget(Vector2 from, float maxRange)
        {
            NPC closest = null;
            float bestSq = maxRange * maxRange;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n.CanBeChasedBy())
                {
                    float d = Vector2.DistanceSquared(from, n.Center);
                    if (d < bestSq && Collision.CanHitLine(Projectile.Center, 1, 1, n.Center, 1, 1))
                    {
                        bestSq = d;
                        closest = n;
                    }
                }
            }
            return closest;
        }

        private void Shatter()
        {
            if (Projectile.localAI[0] != 0f)
                return;

            Projectile.localAI[0] = 1f;

            if (Projectile.owner == Main.myPlayer)
            {
                int shardCount = 5;
                for (int i = 0; i < shardCount; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(4.5f, 4.5f);
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        vel,
                        ModContent.ProjectileType<GaiasShard>(),
                        (int)(Projectile.damage * 0.25f),
                        Projectile.knockBack,
                        Projectile.owner
                    );
                }
            }
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(Projectile.Center, 1, 1, DustID.TerraBlade, 0f, 0f, 0, default, 1.2f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = Main.rand.NextVector2Circular(2f, 2f);
            }
            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.Center, 0, 0, DustID.TerraBlade, 0, 0, 120, Color.LimeGreen, 1.2f);
                Main.dust[d].noGravity = true;
            }

            Projectile.Kill();
        }
    }
}
