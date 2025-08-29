using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CodeBolt : ModProjectile
    {
        private const float HOMING_STRENGTH = 0.5f;
        private const float MAX_HOMING_DISTANCE = 900f;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            NPC target = null;
            float maxDistance = MAX_HOMING_DISTANCE;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly)
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < maxDistance)
                    {
                        maxDistance = distance;
                        target = npc;
                    }
                }
            }

            if (target != null)
            {
                Vector2 targetDirection = target.Center - Projectile.Center;
                targetDirection.Normalize();
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetDirection * Projectile.velocity.Length(), HOMING_STRENGTH);
            }

            Projectile.rotation += 0.4f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch);
            Lighting.AddLight(Projectile.Center, 0f, 1f, 0f);
        }
    }
}
