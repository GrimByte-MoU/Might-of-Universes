using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class VaporBomb : ModProjectile
    {
        private int timer = 0;
        private bool triggered = false;
        private NPC target;

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.timeLeft = 600; // 10 seconds
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            timer++;

            if (!triggered)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, Projectile.Center) < 96f) // 6 tiles
                    {
                        triggered = true;
                        target = npc;
                        break;
                    }
                }
            }
            else if (target != null && target.active)
            {
                if (timer % 10 == 0 && timer <= 30) // Hits 3 times
                {
                    int dmg = Projectile.damage;
                    var hitInfo = new NPC.HitInfo()
                    {
                        Damage = dmg,
                        Knockback = 0f,
                        HitDirection = 0,
                        Crit = false
                    };
                    target.StrikeNPC(hitInfo);
                    target.AddBuff(ModContent.BuffType<Buffs.CodeDestabilized>(), 120); // 2 sec
                }

                if (timer >= 35)
                {
                    Explode();
                }
            }

            if (!triggered && timer >= 600)
            {
                Explode();
            }

            // Vaporwave pink/purple dust pulse
            if (Main.rand.NextBool(2))
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.PinkTorch, Vector2.Zero, 0, Color.Pink, 1.5f).noGravity = true;
            }
        }

        private void Explode()
        {
            for (int i = 0; i < 18; i++)
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(3f, 3f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.MagicMirror, velocity, 0, Main.rand.NextBool(2) ? Color.Red : Color.Purple, 1.5f);
                dust.noGravity = true;
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item74, Projectile.Center); // poof!
            Projectile.Kill();
        }
    }
}
