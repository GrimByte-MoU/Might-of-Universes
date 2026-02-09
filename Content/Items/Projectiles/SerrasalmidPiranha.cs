using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Weapons;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SerrasalmidPiranha : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.alpha = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = -1f;
            Projectile.ai[1] = 0f;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            bool holdingSerrasalmid = owner.active && !owner.dead && !owner.noItems &&
                                      owner.HeldItem.type == ModContent.ItemType<SerrasalmidGun>();
            bool channeling = holdingSerrasalmid && owner.channel;

            if (!channeling && Projectile.ai[1] != 2f)
            {
                StartRecall();
            }

            if (channeling && Projectile.ai[1] == 2f)
            {
                ResumeFromRecall(owner);
            }

            if (channeling)
                Projectile.timeLeft = 2;

            if (Projectile.velocity.LengthSquared() > 0.01f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.ai[1] == 2f)
            {
                DoRecall(owner);
                return;
            }

            int targetIndex = (int)Projectile.ai[0];

            if (Projectile.ai[1] == 0f)
            {
                if (targetIndex < 0 || !ValidTarget(targetIndex))
                {
                    targetIndex = AcquireTarget(Projectile.Center, 900f);
                    Projectile.ai[0] = targetIndex;
                }

                float speed = 14f;
                float inertia = 18f;

                Vector2 desiredVel;
                if (targetIndex >= 0)
                {
                    NPC t = Main.npc[targetIndex];
                    Vector2 to = t.Center - Projectile.Center;
                    float dist = to.Length();
                    desiredVel = dist > 8f ? to.SafeNormalize(Vector2.UnitX) * speed : Projectile.velocity;
                }
                else
                {
                    desiredVel = Projectile.velocity.SafeNormalize(Vector2.UnitX) * speed;
                }

                Projectile.velocity = (Projectile.velocity * (inertia - 1f) + desiredVel) / inertia;
            }
            else if (Projectile.ai[1] == 1f)
            {
                // Latched
                if (targetIndex < 0 || !ValidTarget(targetIndex))
                {
                    StartRecall();
                    return;
                }

                NPC t = Main.npc[targetIndex];
                Vector2 offset = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
                Projectile.Center = t.Center + offset;

                Projectile.position += new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f));
                Projectile.velocity *= 0.95f;

                Vector2 look = t.Center - Projectile.Center;
                if (look.LengthSquared() > 0.001f)
                    Projectile.rotation = look.ToRotation() + MathHelper.PiOver2;
            }
        }

        private void StartRecall()
        {
            Projectile.ai[1] = 2f;
            Projectile.ai[0] = -1f;
            Projectile.friendly = false;
            Projectile.netUpdate = true;

            if (Projectile.timeLeft < 180)
                Projectile.timeLeft = 180;
        }

        private void ResumeFromRecall(Player owner)
        {
            Projectile.ai[1] = 0f;
            Projectile.ai[0] = -1f;
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.timeLeft = 2;
            Projectile.netUpdate = true;
            Vector2 dir = Main.MouseWorld - owner.MountedCenter;
            if (dir.LengthSquared() < 0.01f)
                dir = Vector2.UnitX.RotatedByRandom(0.5f);
            dir.Normalize();
            Projectile.velocity = dir * 14f;
        }

        private void DoRecall(Player owner)
        {
            Vector2 toOwner = owner.MountedCenter - Projectile.Center;
            float dist = toOwner.Length();
            float speed = 18f;
            float inertia = 16f;

            Vector2 desired = toOwner.SafeNormalize(Vector2.UnitX) * speed;
            Projectile.velocity = (Projectile.velocity * (inertia - 1f) + desired) / inertia;

            if (Projectile.velocity.LengthSquared() > 0.01f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.alpha = (byte)MathHelper.Clamp(Projectile.alpha + 4, 0, 255);

            if (dist < 24f)
                Projectile.Kill();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[1] == 2f)
                return;

            if (Projectile.ai[1] == 0f && target.CanBeChasedBy())
            {
                Projectile.ai[1] = 1f;
                Projectile.ai[0] = target.whoAmI;
                Vector2 offset = Projectile.Center - target.Center;
                Projectile.localAI[0] = offset.X;
                Projectile.localAI[1] = offset.Y;
                Projectile.netUpdate = true;
            }
            if (hit.Crit)
            {
                target.AddBuff(ModContent.BuffType<Tarred>(), 30);
            }
        }

        private static bool ValidTarget(int idx)
        {
            if (idx < 0 || idx >= Main.maxNPCs) return false;
            NPC n = Main.npc[idx];
            return n.active && !n.friendly && !n.dontTakeDamage && n.CanBeChasedBy();
        }

        private static int AcquireTarget(Vector2 from, float maxRange)
        {
            int best = -1;
            float bestDist = maxRange;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (!n.active || n.friendly || n.dontTakeDamage || !n.CanBeChasedBy()) continue;

                float d = Vector2.Distance(from, n.Center);
                if (d < bestDist && Collision.CanHitLine(from, 1, 1, n.Center, 1, 1))
                {
                    best = i;
                    bestDist = d;
                }
            }
            return best;
        }
    }
}