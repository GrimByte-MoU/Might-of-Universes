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
            // Clone Gungnir projectile
            Projectile.CloneDefaults(ProjectileID.Gungnir);
            AIType = ProjectileID.Gungnir;
        }

        public override void AI()
        {
            base.AI();
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // ONLY inflict Rebuking Light for 3 seconds (no other debuffs)
            target.AddBuff(ModContent.BuffType<RebukingLight>(), 180);
        }
    }
}