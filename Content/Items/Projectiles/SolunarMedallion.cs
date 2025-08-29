using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SolunarMedallion : ModProjectile
    {
        private float rotation = 0f;
        private const float ORBIT_RADIUS = 150f;
        private const float ROTATION_SPEED = 0.075f;
        private bool isSunMedallion;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.damage = 30;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            
            if (!reaperPlayer.hasReaperArmor)
            {
                Projectile.Kill();
                return;
            }
            
            rotation += ROTATION_SPEED;
            if (Projectile.ai[0] == 1f)
                rotation += MathHelper.Pi;

            Vector2 offset = new Vector2(
                ORBIT_RADIUS * (float)System.Math.Cos(rotation),
                ORBIT_RADIUS * (float)System.Math.Sin(rotation)
            );

            Projectile.Center = player.Center + offset;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.AddSoulEnergy(1f, target.Center);
        }
    }
}
