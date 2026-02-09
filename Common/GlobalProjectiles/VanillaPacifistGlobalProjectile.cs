using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Common.GlobalProjectiles
{
    public class VanillaPacifistGlobalProjectile : GlobalProjectile
    {
        private static readonly int[] PacifistProjectileIDs = new int[]
        {
            ProjectileID.StarVeilStar,
            ProjectileID.StarCloakStar,
            ProjectileID.Bee,
            ProjectileID.GiantBee,
            ProjectileID.Wasp,
            ProjectileID.SporeCloud,
            ProjectileID.SporeGas,
            ProjectileID.SporeGas2,
            ProjectileID.SporeGas3,
            ProjectileID.VolatileGelatinBall,
            ProjectileID.BoneGloveProj,
            ProjectileID.FlamingJack,
            ProjectileID.BoneJavelin,
        };

        public override void SetDefaults(Projectile projectile)
        {
            foreach (int id in PacifistProjectileIDs)
            {
                if (projectile.type == id)
                {
                    projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
                    break;
                }
            }
        }
    }
}