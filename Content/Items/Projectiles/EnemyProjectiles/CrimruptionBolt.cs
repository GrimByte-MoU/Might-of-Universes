using Microsoft. Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class CrimruptionBolt : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.alpha = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            float pulseScale = 1f + (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.1f;
            Projectile.scale = pulseScale;

            Lighting.AddLight(Projectile.Center, 0.6f, 0.3f, 0.8f);

            if (Main.rand. NextBool(2))
            {
                int dustType = Main.rand.Next(3);
                int dustID;
                Color dustColor = default;

                switch (dustType)
                {
                    case 0:
                        dustID = DustID.Corruption;
                        dustColor = Color. Purple;
                        break;
                    case 1:
                        dustID = DustID.Blood;
                        dustColor = Color.Red;
                        break;
                    default:
                        dustID = DustID.CursedTorch;
                        dustColor = Color.LimeGreen;
                        break;
                }

                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    dustID,
                    0f, 0f, 100,
                    dustColor,
                    0.8f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
{
    modifiers.FinalDamage.Base = Projectile.damage;
}


        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int ichorDuration = 120;

            if (Main.expertMode)
                ichorDuration = 180;

            if (Main.masterMode)
                ichorDuration = 240;

            target.AddBuff(BuffID.Ichor, ichorDuration);

            int cursedDuration = 120;

            if (Main.expertMode)
                cursedDuration = 180;

            if (Main.masterMode)
                cursedDuration = 240;

            target.AddBuff(BuffID.CursedInferno, cursedDuration);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                int dustType = Main.rand.Next(3);
                int dustID = dustType == 0 ? DustID. Corruption : dustType == 1 ? DustID.Blood : DustID. CursedTorch;

                Dust dust = Dust. NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    dustID,
                    Main.rand.NextFloat(-4f, 4f),
                    Main.rand.NextFloat(-4f, 4f),
                    100, default(Color), 1.2f
                );
                dust. noGravity = true;
            }

            // Green flash
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.CursedTorch,
                    Main.rand.NextVector2Circular(3, 3),
                    100, Color.LimeGreen, 1.5f
                );
            }
        }

        public override Color?  GetAlpha(Color lightColor)
        {
            return new Color(150, 100, 200, 200);
        }
    }
}