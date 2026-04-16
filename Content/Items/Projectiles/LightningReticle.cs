using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class LightningReticle : ModProjectile
    {
        private ref float TargetNPC => ref Projectile.ai[0];
        private int delayTimer = 0;
        private const int DELAY_TIME = 60;
        private int boltsFired = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 100;
        }

        public override void AI()
        {
            if (TargetNPC >= 0 && TargetNPC < Main.maxNPCs)
            {
                NPC target = Main.npc[(int)TargetNPC];
                if (target.active)
                {
                    Projectile.Center = target.Center;
                }
                else
                {
                    TargetNPC = -1;
                }
            }
            else
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                    {
                        float distance = Vector2.Distance(Projectile.Center, npc.Center);
                        if (distance < 50f)
                        {
                            TargetNPC = i;
                            break;
                        }
                    }
                }
            }

            delayTimer++;

            Projectile.rotation += 0.1f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            if (delayTimer == DELAY_TIME || delayTimer == DELAY_TIME + 15)
            {
                FireLightningBolt();
            }

            if (delayTimer > DELAY_TIME + 20)
            {
                Projectile.Kill();
            }
        }

        private void FireLightningBolt()
        {
            if (Main.myPlayer != Projectile.owner)
                return;

            Player player = Main.player[Projectile.owner];
            
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<LightningBolt>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner
            );

            SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
            
            boltsFired++;
        }

        public override bool? CanDamage() => false;
    }
}