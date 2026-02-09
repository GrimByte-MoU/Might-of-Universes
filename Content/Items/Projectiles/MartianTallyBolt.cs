using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using System;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MartianTallyBolt : MoUProjectile
    {
        private float zigzagTimer = 0f;
        private const float ZIGZAG_SPEED = 0.2f;
        private const float ZIGZAG_WIDTH = 60f;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 3;
            Projectile.timeLeft = 240;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = Projectile.velocity.ToRotation();
                Projectile.ai[1] = Projectile.velocity.Length();
            }

            zigzagTimer += ZIGZAG_SPEED;
            float baseRotation = Projectile.ai[0];            
            Vector2 baseDirection = new Vector2((float)Math.Cos(baseRotation), (float)Math.Sin(baseRotation));
            Vector2 perpendicular = new Vector2(-baseDirection.Y, baseDirection.X);
            
            Projectile.velocity = baseDirection * Projectile.ai[1] + perpendicular * (float)Math.Cos(zigzagTimer) * 4f;
            
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            if (Main.rand.NextBool(2))
            {
                Dust neonGreen = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.GreenTorch, 
                    0f, 0f, 0, default, 1.2f);
                neonGreen.noGravity = true;
                neonGreen.velocity *= 0.3f;
            }
            
            if (Main.rand.NextBool(2))
            {
                Dust electric = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.Electric, 
                    0f, 0f, 0, default, 1f);
                electric.noGravity = true;
                electric.velocity *= 0.3f;
            }
            
            Lighting.AddLight(Projectile.Center, 0.3f, 1f, 0.3f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center);
            
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(target.position, target.width, target.height, DustID.Electric);
                Dust.NewDust(target.position, target.width, target.height, DustID.GreenTorch);
            }
        }
    }
}