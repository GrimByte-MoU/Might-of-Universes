using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;


namespace MightofUniverses.Content.Items.Projectiles
{
    public class CobaltScytheProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        { // Makes projectile face its movement direction
        Projectile.rotation = Projectile.velocity.ToRotation();

// Optional: Add this if you want the sprite to be oriented differently
// //Projectile.rotation += MathHelper.PiOver2; // Rotates sprite 90 degrees  
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cobalt);
            }
            Projectile.spriteDirection = Projectile.direction;

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(2f, target.Center);
        }
    }
}