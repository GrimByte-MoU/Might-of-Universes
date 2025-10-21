using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ViralFragment : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 6;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 120;
            Projectile.light = 0.25f;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Red.ToVector3() * 0.6f);
            //Projectile.rotation += 0.4f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Buffs.CodeDestabilized>(), 180); // 3 seconds
        }
    }
}
