using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Common.GlobalProjectiles
{
    public class PacifistGlobalProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            // If the projectile uses Pacifist damage type, give it vanilla‐style iframes
            if (projectile.DamageType == ModContent.GetInstance<PacifistDamageClass>())
            {
                projectile.usesLocalNPCImmunity = false;
                projectile.usesIDStaticNPCImmunity = true;
                projectile.idStaticNPCHitCooldown = 10;
            }
        }
    }
}
