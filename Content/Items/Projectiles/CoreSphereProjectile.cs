using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;
using Terraria.ID;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CoreSphereProjectile : MoUProjectile
    {
        private float rotation = 0f;
        private const float ORBIT_RADIUS = 200f;
        private const float ROTATION_SPEED = -0.15f;

        public override void SafeSetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override bool? CanHitNPC(NPC target) => true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            
            int index = (int)Projectile.ai[0];
            float baseAngle = index / 6f * MathHelper.TwoPi;
            
            rotation += ROTATION_SPEED;

            Vector2 offset = new Vector2(
                ORBIT_RADIUS * (float)System.Math.Cos(baseAngle + rotation),
                ORBIT_RADIUS * (float)System.Math.Sin(baseAngle + rotation)
            );

            Projectile.Center = player.Center + offset;
            Projectile.rotation += 0.15f;
            
            if (!player.GetModPlayer<CoreSpherePlayer>().hasCoreSphere)
            {
                Projectile.Kill();
            }

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    0f,
                    0f,
                    100,
                    Color.Orange,
                    1.2f
                );
                dust.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 0.8f, 0.4f, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CoreHeat>(), 120);
        }
    }
}