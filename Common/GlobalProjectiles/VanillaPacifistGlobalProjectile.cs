using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Common.GlobalProjectiles
{
    /// <summary>
    /// Reclasses vanilla accessory projectiles to PacifistDamageClass
    /// </summary>
    public class VanillaPacifistGlobalProjectile : GlobalProjectile
    {
        // Projectile IDs that should deal Pacifist damage
        private static readonly int[] PacifistProjectileIDs = new int[]
        {
            ProjectileID.StarVeilStar,          // Star Veil
            ProjectileID.StarCloakStar,         // Star Cloak / Mana Cloak (same projectile ID)
            ProjectileID.Bee,                   // Bee Cloak / Honey Comb
            ProjectileID.GiantBee,              // Sweetheart Necklace
            ProjectileID.Wasp,                  // Stinger Necklace
            ProjectileID.SporeCloud,            // Spore Sac
            ProjectileID.SporeGas,              // Spore Sac (alternate)
            ProjectileID.SporeGas2,             // Spore Sac (alternate 2)
            ProjectileID.SporeGas3,             // Spore Sac (alternate 3)
            ProjectileID.VolatileGelatinBall,   // Volatile Gelatin
            ProjectileID.BoneGloveProj,         // Bone Glove
            ProjectileID.FlamingJack,           // Bone Helm (flaming jack-o'-lanterns)
            ProjectileID.BoneJavelin,           // Bone Helm (alternate)
        };

        public override void SetDefaults(Projectile projectile)
        {
            // Check if this projectile should be reclassed to Pacifist
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