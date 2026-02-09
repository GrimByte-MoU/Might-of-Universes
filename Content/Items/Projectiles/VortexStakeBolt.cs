using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class VortexStakeBolt : MoUProjectile
    {
        private const float HELIX_RADIUS = 20f;
        private const float HELIX_SPEED = 0.2f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            
            if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = Projectile.velocity.Length();
                Projectile.netUpdate = true;
            }
            
            Projectile.ai[0] += HELIX_SPEED;
            Vector2 forwardDir = Projectile.velocity.SafeNormalize(Vector2.Zero);
            Vector2 perpDir = forwardDir.RotatedBy(MathHelper.PiOver2);
            float offset = (float)Math.Sin(Projectile.ai[0]) * HELIX_RADIUS;
            Vector2 newVelocity = forwardDir * Projectile.ai[1] + perpDir * offset * HELIX_SPEED;
            Projectile.velocity = newVelocity;
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Vortex,
                    0f,
                    0f,
                    100,
                    default,
                    1f + Main.rand.NextFloat() * 0.4f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                
                if (Main.rand.NextBool(3))
                {
                    Dust dust2 = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.Electric,
                        0f,
                        0f,
                        100,
                        default,
                        0.5f + Main.rand.NextFloat() * 0.3f
                    );
                    dust2.noGravity = true;
                    dust2.velocity *= 0.2f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 15; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Vortex,
                    speed * 4f,
                    Scale: 1.2f + Main.rand.NextFloat() * 0.5f
                );
                dust.noGravity = true;
            }
            
            SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Vortex,
                    speed * 3f,
                    Scale: 1f + Main.rand.NextFloat() * 0.4f
                );
                dust.noGravity = true;
            }
            
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            
            return true;
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPosEffect = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
                Color color = Projectile.GetAlpha(Color.Lerp(new Color(180, 80, 255), new Color(0, 80, 255), k / (float)Projectile.oldPos.Length)) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                float scale = Projectile.scale * (1f - k / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(
                    texture,
                    drawPosEffect,
                    null,
                    color,
                    Projectile.oldRot[k],
                    drawOrigin,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }
            
            Color projectileColor = Projectile.GetAlpha(new Color(180, 80, 255));
            Main.spriteBatch.Draw(
                texture,
                drawPos,
                null,
                projectileColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
            
            return false;
        }
    }
}


