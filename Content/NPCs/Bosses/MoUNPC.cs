using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Content;

namespace MightofUniverses.Content.NPCs
{
    public abstract class MoUNPC : ModNPC
    {
        public virtual void SafeSetDefaults() { }
        public virtual bool SafePreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => true;

        public sealed override void SetDefaults()
        {
            AutoSetHitboxFromSprite();
            SafeSetDefaults();
        }

        public sealed override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!SafePreDraw(spriteBatch, screenPos, drawColor))
                return false;

            Asset<Texture2D> tex = ModContent.Request<Texture2D>(Texture);
            Texture2D texture = tex.Value;
            Rectangle? sourceRect = null;
            Vector2 drawOrigin;

            if (Main.npcFrameCount[NPC.type] > 1)
            {
                int frameHeight = texture.Height / Main.npcFrameCount[NPC.type];
                sourceRect = new Rectangle(0, frameHeight * NPC.frame.Y / frameHeight, texture.Width, frameHeight);
                drawOrigin = sourceRect.Value.Size() * 0.5f;
            }
            else
            {
                drawOrigin = texture.Size() * 0.5f;
            }

            Vector2 drawPos = NPC.Center - screenPos;
            Color color = drawColor;
            spriteBatch.Draw(
                texture,
                drawPos,
                sourceRect,
                color,
                NPC.rotation,
                drawOrigin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }

        private void AutoSetHitboxFromSprite()
        {
            if (ModContent.RequestIfExists<Texture2D>(Texture, out var tex))
            {
                Texture2D texture = tex.Value;
                NPC.width = texture.Width;
                NPC.height = texture.Height;
            }
            else
            {
                NPC.width = 32;
                NPC.height = 32;
            }
        }
    }
}