using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WorldwalkerStinger : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 90;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.99f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.JungleSpore);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.gfxOffY = -32f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Venom, 300);
            target.AddBuff(BuffID.Poisoned, 300);
        }
    }
}
