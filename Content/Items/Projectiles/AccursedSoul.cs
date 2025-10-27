using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class AccursedSoul : MoUProjectile
    {
        private const float DetectionRadius = 320f;
        private const float HomingSpeed = 8f;
        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                NPC target = null;
                float best = DetectionRadius;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && !npc.townNPC && !npc.immortal && npc.lifeMax > 5)
                    {
                        float d = Vector2.Distance(npc.Center, Projectile.Center);
                        if (d < best)
                        {
                            best = d;
                            target = npc;
                        }
                    }
                }
                if (target != null)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = target.whoAmI;
                    Vector2 dir = target.Center - Projectile.Center;
                    dir.Normalize();
                    Projectile.velocity = dir * HomingSpeed;
                }
                else
                {
                    Projectile.velocity *= 0.95f;
                    if (Main.rand.NextBool(10))
                    {
                        int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleCrystalShard, 0f, 0f, 100, default, 0.8f);
                        Main.dust[d].noGravity = true;
                    }
                }
            }
            else
            {
                int idx = (int)Projectile.ai[1];
                if (idx >= 0 && idx < Main.maxNPCs)
                {
                    var target = Main.npc[idx];
                    if (target != null && target.active && !target.friendly && !target.townNPC)
                    {
                        Vector2 desired = target.Center - Projectile.Center;
                        desired.Normalize();
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired * HomingSpeed, 0.12f);
                    }
                    else
                    {
                        Projectile.ai[0] = 0f;
                        Projectile.ai[1] = 0f;
                        Projectile.velocity = Vector2.Zero;
                    }
                }
                else
                {
                    Projectile.ai[0] = 0f;
                    Projectile.ai[1] = 0f;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            float pulse = 0.7f + 0.3f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6f + Projectile.whoAmI);
            float r = 0.02f * pulse;
            float g = 0.55f * pulse;
            float b = 0.08f * pulse;
            Lighting.AddLight(Projectile.Center, r, g, b);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player owner = Main.player[Projectile.owner];
            if (owner != null && owner.active)
            {
                var reaper = owner.GetModPlayer<ReaperPlayer>();
                reaper?.AddSoulEnergyFromNPC(target, 2f);
            }
            for (int i = 0; i < 8; i++)
            {
                int d = Dust.NewDust(target.position, target.width, target.height, DustID.CursedTorch, 0f, 0f, 100, default, 1f);
                Main.dust[d].noGravity = true;
            }
            Projectile.Kill();
        }
    }
}