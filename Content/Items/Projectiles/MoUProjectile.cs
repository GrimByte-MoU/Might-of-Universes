using Microsoft.Xna.Framework;
using Microsoft. Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public abstract class MoUProjectile :  ModProjectile
    {
        public virtual void SafeSetDefaults() { }

        public virtual bool SafePreDraw(ref Color lightColor) => true;

        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
        }

        public sealed override bool PreDraw(ref Color lightColor)
        {
            if (!SafePreDraw(ref lightColor))
                return false;

            var tex = ModContent.Request<Texture2D>(Texture);
            Texture2D texture = tex.Value;

            Rectangle? sourceRect = null;
            Vector2 drawOrigin;

            if (Main.projFrames[Projectile.type] > 1)
            {
                int frameHeight = texture.Height / Main.projFrames[Projectile.type];
                sourceRect = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
                drawOrigin = sourceRect.Value.Size() * 0.5f;
            }
            else
            {
                drawOrigin = texture.Size() * 0.5f;
            }

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color drawColor = lightColor * (1f - Projectile.alpha / 255f);

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                sourceRect,
                drawColor,
                Projectile.rotation,
                drawOrigin,
                Projectile. scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}