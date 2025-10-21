using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FungShuiSpore2 : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<FungShuiDisturbance>(), 60);
        }
    }
}