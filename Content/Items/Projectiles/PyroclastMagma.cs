using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PyroclastMagma : MoUProjectile
    {
        private NPC targetNPC;
        private int targetWhoAmI = -1;
        private const int MaxLifetime = 180;

        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = MaxLifetime;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] >= 0)
            {
                targetWhoAmI = (int)Projectile.ai[0];
            }

            if (targetWhoAmI >= 0 && targetWhoAmI < Main.maxNPCs)
            {
                targetNPC = Main.npc[targetWhoAmI];

                if (!targetNPC.active || targetNPC.life <= 0)
                {
                    Projectile.Kill();
                    return;
                }

                Projectile.Center = targetNPC.Center;
            }
            else
            {
                Projectile.Kill();
                return;
            }

            Projectile.rotation += 0.1f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0f, 0f, 100, default, 2.0f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(2f, 2f);
            }

            if (Main.rand.NextBool(2))
            {
                Dust fire = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, -1f, 100, Color.OrangeRed, 1.8f);
                fire.noGravity = true;
                fire.velocity.Y -= 2f;
            }

            Lighting.AddLight(Projectile.Center, 1.2f, 0.6f, 0.3f);

            float fadeProgress = 1f - (Projectile.timeLeft / (float)MaxLifetime);
            Projectile.alpha = (int)(255 * fadeProgress);
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            float fadeAlpha = 1f - (Projectile.alpha / 255f);

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                Color.White * fadeAlpha * 0.8f,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale * 1.2f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor * fadeAlpha,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CoreHeat>(), 300);

            if (Main.rand.NextBool(3))
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Torch, 0f, 0f, 100, Color.Red, 1.5f);
                    dust.noGravity = true;
                    dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0f, 0f, 100, default, 2.0f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
            }
        }
    }
}