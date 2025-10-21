using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FloatingHeartCrystal : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.3f, 0.3f);
        }

        public override bool? CanHitNPC(NPC target) => false;

        public override void Kill(int timeLeft)
        {
            Player owner = Main.player[Projectile.owner];
            owner.statLife += 25;
            owner.HealEffect(25);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Palladium);
            }
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;
    }
}

