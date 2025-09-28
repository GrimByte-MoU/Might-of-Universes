using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalProjectiles
{
    public class VanillaBalanceGlobalProjectile : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            switch (projectile.type)
            {
                case ProjectileID.AdamantiteGlaive:
                    if (Main.rand.NextBool(6))
                    {
                        int d = Dust.NewDust(projectile.Center, 1, 1, DustID.Electric, 0f, 0f, 150, default, 0.9f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 0.2f;
                    }
                    break;

                case ProjectileID.ChlorophytePartisan:
                    if (Main.rand.NextBool(6))
                    {
                        int d = Dust.NewDust(projectile.Center, 1, 1, DustID.GreenTorch, 0f, 0f, 150, default, 0.9f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 0.2f;
                    }
                    break;

                case ProjectileID.SporeCloud:
                    if (Main.rand.NextBool(6))
                    {
                        int d = Dust.NewDust(projectile.Center, 1, 1, DustID.Grass, 0f, 0f, 150, default, 0.9f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 0.15f;
                    }
                    break;

                case ProjectileID.TitaniumTrident:
                    if (Main.rand.NextBool(6))
                    {
                        int d = Dust.NewDust(projectile.Center, 1, 1, DustID.Torch, 0f, 0f, 150, default, 1f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 0.2f;
                    }
                    break;
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            switch (projectile.type)
            {
                case ProjectileID.AdamantiteGlaive:
                    target.AddBuff(BuffID.Electrified, 300);
                    break;

                case ProjectileID.ChlorophytePartisan:
                    target.AddBuff(BuffID.Venom, 180);
                    break;

                case ProjectileID.SporeCloud:
                    target.AddBuff(BuffID.Venom, 120);
                    break;

                case ProjectileID.TitaniumTrident:
                    target.AddBuff(BuffID.OnFire3, 300);
                    break;
            }
        }
    }
}