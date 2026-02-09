using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SapphireMinion : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override bool? CanCutTiles() => false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.HasBuff(ModContent.BuffType<Buffs.SapphireGlaiveBuff>()))
            {
                Projectile.Kill();
                return;
            }

            float idleRadius = 80f;
            float idleSpeed = 6f;
            float targetRadius = 240f;
            int targetIndex = (int)Projectile.ai[1];
            NPC target = null;

            if (targetIndex > 0 && targetIndex < Main.maxNPCs)
            {
                NPC possible = Main.npc[targetIndex];
                if (possible.active && !possible.friendly && possible.CanBeChasedBy(this))
                {
                    target = possible;
                }
                else
                {
                    Projectile.ai[1] = -1;
                }
            }

            if (target == null)
            {
                float closestDist = targetRadius;
                int closestIndex = -1;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.CanBeChasedBy(this))
                    {
                        float dist = Vector2.Distance(Projectile.Center, npc.Center);
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            closestIndex = i;
                        }
                    }
                }
                if (closestIndex != -1)
                {
                    Projectile.ai[1] = closestIndex;
                    target = Main.npc[closestIndex];
                }
                else
                {
                    Projectile.ai[1] = -1;
                }
            }

            if (target != null)
            {
                float aboveOffset = -60f;
                float bobbing = (float)System.Math.Sin(Main.GameUpdateCount / 20f + Projectile.whoAmI) * 16f;
                float drift = (float)System.Math.Cos(Main.GameUpdateCount / 45f + Projectile.whoAmI) * 14f;

                Vector2 desiredPosition = target.Center + new Vector2(drift, aboveOffset + bobbing);

                Vector2 toDesired = desiredPosition - Projectile.Center;
                float moveSpeed = 12f;
                if (toDesired.Length() > moveSpeed)
                    toDesired = toDesired.SafeNormalize(Vector2.Zero) * moveSpeed;

                Projectile.velocity = (Projectile.velocity * 14f + toDesired) / 15f;

                if (++Projectile.localAI[0] >= 30)
                {
                    Projectile.localAI[0] = 0;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Vector2 shootVel = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 8f;
                        Projectile.NewProjectile(
                            Projectile.GetSource_FromAI(),
                            Projectile.Center,
                            shootVel,
                            ModContent.ProjectileType<LightGemProjectile>(),
                            Projectile.damage,
                            0f,
                            Projectile.owner);
                    }
                }
            }
            else
            {
                if (Projectile.ai[2] == 0 || Vector2.Distance(Projectile.Center, player.Center + new Vector2(Projectile.localAI[1], Projectile.localAI[2])) < 16f)
                {
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    float dist = Main.rand.NextFloat(idleRadius * 0.5f, idleRadius);
                    Vector2 wanderOffset = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * dist;
                    Projectile.localAI[1] = wanderOffset.X;
                    Projectile.localAI[2] = wanderOffset.Y;
                    Projectile.ai[2] = Main.rand.Next(40, 120);
                }
                else
                {
                    Projectile.ai[2]--;
                }

                Vector2 idleTarget = player.Center + new Vector2(Projectile.localAI[1], Projectile.localAI[2]);
                Vector2 toIdle = idleTarget - Projectile.Center;
                float distance = toIdle.Length();
                if (distance > 4f)
                {
                    toIdle.Normalize();
                    Projectile.velocity = (Projectile.velocity * 20f + toIdle * idleSpeed) / 21f;
                }
                else
                {
                    Projectile.velocity *= 0.9f;
                }
            }
        }
    }
}