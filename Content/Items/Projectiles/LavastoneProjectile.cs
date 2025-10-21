using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;
using Terraria.ID;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class LavastoneProjectile : MoUProjectile
    {
        private float rotation = 0f;
        private const float ORBIT_RADIUS = 60f;
        private const float ROTATION_SPEED = 0.075f;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Projectile.damage = 15;
        }

public override bool? CanHitNPC(NPC target) => true;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            
            rotation += ROTATION_SPEED;

            Vector2 offset = new Vector2(
                ORBIT_RADIUS * (float)System.Math.Cos(rotation),
                ORBIT_RADIUS * (float)System.Math.Sin(rotation)
            );

            Projectile.Center = player.Center + offset;
            
            if (!player.GetModPlayer<LavastonePlayer>().hasLavastone)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }
    }
}