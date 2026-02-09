using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Common.GlobalProjectiles
{
    public class PacifistGlobalProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.DamageType == ModContent.GetInstance<PacifistDamageClass>())
            {
                projectile.usesLocalNPCImmunity = false;
                projectile.usesIDStaticNPCImmunity = true;
                projectile.idStaticNPCHitCooldown = 10;
            }
        }
    }
}
