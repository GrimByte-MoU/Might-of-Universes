using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EclipseSphere : MoUProjectile
    {
        private float rotation = 0f;
        private const float ORBIT_RADIUS = 200f;
        private const float ROTATION_SPEED = 0.075f;
        
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.damage = 70;
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
            float angleOffset = Projectile.ai[0] * MathHelper.PiOver2;
            float currentRotation = rotation + angleOffset;

            Vector2 offset = new Vector2(
                ORBIT_RADIUS * (float)System.Math.Cos(currentRotation),
                ORBIT_RADIUS * (float)System.Math.Sin(currentRotation)
            );

            Projectile.Center = player.Center + offset;
Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0f);

if (Main.rand.NextBool(2))
{
    Dust dust = Dust.NewDustDirect(
        Projectile.position,
        Projectile.width,
        Projectile.height,
        DustID.OrangeTorch,
        0f,
        0f,
        100,
        default,
        1.5f
    );
    dust.noGravity = true;
    dust.velocity *= 0.3f;

    if (Main.rand.NextBool(3))
    {
        Dust fireDust = Dust.NewDustDirect(
            Projectile.position,
            Projectile.width,
            Projectile.height,
            DustID.Torch,
            0f,
            0f,
            100,
            default,
            1.2f
        );
        fireDust.noGravity = true;
        fireDust.velocity *= 0.5f;
    }
}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.AddSoulEnergy(0.4f, target.Center);
        }
    }
}
