using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    /// <summary>
    /// Base class for all Might of Universes projectiles.
    /// Automatically handles sprite centering and hitbox sizing.
    /// 
    /// HOW TO USE:
    /// 1. Inherit from MoUProjectile instead of MoUProjectile
    /// 2. Use SafeSetDefaults() instead of SetDefaults()
    /// 3. DO NOT set Projectile.width or Projectile.height (auto-detected from sprite!)
    /// 4. Rotation will work perfectly around the sprite center
    /// </summary>
    public abstract class MoUProjectile : ModProjectile
    {
        /// <summary>
        /// Override this instead of SetDefaults.
        /// Width and height are automatically set from your sprite dimensions!
        /// </summary>
        public virtual void SafeSetDefaults() { }

        /// <summary>
        /// Override this if you need custom drawing behavior.
        /// By default, sprites are perfectly centered for rotation.
        /// Return true to allow the built-in centered draw to execute,
        /// return false if you fully handle drawing inside SafePreDraw.
        /// </summary>
        public virtual bool SafePreDraw(ref Color lightColor) => true;

        public sealed override void SetDefaults()
        {
            // Let tModLoader auto-detect sprite dimensions
            // Call the per-projectile defaults
            SafeSetDefaults();
        }

        public sealed override bool PreDraw(ref Color lightColor)
        {
            // If a child wants to fully override drawing, let it do so.
            if (!SafePreDraw(ref lightColor))
                return false;

            // Default centered draw behavior
            var tex = ModContent.Request<Texture2D>(Texture);
            Texture2D texture = tex.Value;

            Vector2 drawOrigin = texture.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color drawColor = lightColor * (1f - Projectile.alpha / 255f);

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                drawColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            // We drew manually, so return false to prevent default draw
            return false;
        }
    }
}