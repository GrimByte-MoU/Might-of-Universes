using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class AdamantiteSphereProjectile : ModProjectile
    {
        private const float HOMING_STRENGTH = 0.15f;
        private const float MAX_HOMING_DISTANCE = 400f;

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
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Adamantite);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
            Lighting.AddLight(Projectile.Center, 0.8f, 0f, 0.2f);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += 999999;
            modifiers.FinalDamage *= 1f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 600);
        }
    }
}
