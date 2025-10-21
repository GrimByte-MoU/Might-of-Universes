using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class HorrorIchorSplash : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.IchorSplash);
            AIType = ProjectileID.IchorSplash;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 180);
            target.AddBuff(ModContent.BuffType<EnemyBleeding>(), 180);
        }
    }
}