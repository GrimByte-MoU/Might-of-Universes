using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class GaeBulgProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Gungnir);
            AIType = ProjectileID.Gungnir;
            Projectile.scale = 1.5f;
        }

        public override void AI()
        {
            base.AI();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<RebukingLight>(), 180);
        }
    }
}