using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;


namespace MightofUniverses.Content.Items.Projectiles
{
    public class ChlorophyteScytheProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.scale = 1.5f;
        }

        public override void AI()
        {
            
           Projectile.rotation = Projectile.velocity.ToRotation();

// Optional: Add this if you want the sprite to be oriented differently
// Projectile.rotation += MathHelper.PiOver2; // Rotates sprite 90 degrees

            
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Chlorophyte);
            }
            Projectile.spriteDirection = Projectile.direction;

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(3f, target.Center);
        }
    }
}