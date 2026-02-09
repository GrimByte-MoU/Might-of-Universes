using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class BSCUProjectile : MoUProjectile
    {
        private float rotation = 0f;
        private const float ORBIT_RADIUS = 90f;
        private const float ROTATION_SPEED = 0.125f;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Projectile.damage = 45;
        }

public override bool? CanHitNPC(NPC target) => true;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            rotation += ROTATION_SPEED;
            float offsetRotation = rotation + (MathHelper.TwoPi * Projectile.ai[0] / 3f); // 3 spheres

            Vector2 offset = new Vector2(
                ORBIT_RADIUS * (float)System.Math.Cos(offsetRotation),
                ORBIT_RADIUS * (float)System.Math.Sin(offsetRotation)
            );

            Projectile.Center = player.Center + offset;

            if (!player.GetModPlayer<BSCUPlayer>().hasBSCU)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        target.AddBuff(ModContent.BuffType<Shred>(), 120);
        }
    }
}