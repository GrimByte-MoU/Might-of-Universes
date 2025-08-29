// Common/GlobalProjectiles/DawnsPiercerGlobalProjectile.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalProjectiles
{
    public class DawnsPiercerGlobalProjectile : GlobalProjectile
    {
        public bool ApplyDebuffs;

        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ApplyDebuffs)
            {
                target.AddBuff(ModContent.BuffType<Sunfire>(), 180);
                target.AddBuff(BuffID.Daybreak, 180);
            }
        }
    }
}
