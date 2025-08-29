using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EclipseBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3() * 1.5f);

            if (Vector2.Distance(Projectile.Center, Main.player[Projectile.owner].Center) < 24f)
            {
                var player = Main.player[Projectile.owner];
                player.statLife += 5;
                player.HealEffect(5);
                player.AddBuff(ModContent.BuffType<EclipseBlessing>(), 180); // 3 seconds
                Projectile.Kill();
            }
        }
    }
}
