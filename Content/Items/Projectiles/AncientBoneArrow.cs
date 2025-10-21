using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class AncientBoneArrow : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.WoodenArrowFriendly;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 7;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.timeLeft = 600;
            Projectile.ArmorPenetration = 50;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}