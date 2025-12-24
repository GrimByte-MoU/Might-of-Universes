// Content/Items/Accessories/AegisReplicaShieldLayer.cs

using Terraria;
using Terraria. ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Content;

namespace MightofUniverses.Content. Items.Accessories
{
    public class AegisReplicaShieldLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.MountFront);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            var modPlayer = player.GetModPlayer<AegisReplicaPlayer>();

            if (! modPlayer.hasAegisReplica || modPlayer.hideVisual) return;
            if (modPlayer.shieldHealth <= 0) return;

            // Load shield texture
            Texture2D shieldTexture = ModContent.Request<Texture2D>(
                "MightofUniverses/Content/Items/Accessories/AegisReplicaShield",
                AssetRequestMode.ImmediateLoad
            ).Value;

            // Calculate position
            Vector2 drawPosition = player.MountedCenter - Main.screenPosition;
            drawPosition = new Vector2((int)drawPosition.X, (int)drawPosition.Y);

            // Calculate opacity based on shield health
            float healthPercent = modPlayer.shieldHealth / modPlayer.maxShieldHealth;
            
            // CHANGED: Full opacity (1. 0) at 100% HP, fades to 0.0 at 0% HP
            float baseAlpha = healthPercent; // 1.0 → 0.0 linear
            // float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.5f) * 0.05f + 0.95f;
            // float finalAlpha = baseAlpha * pulse;

            // NO PULSE VERSION (completely solid at full HP):
            float finalAlpha = baseAlpha;

            // Slow rotation (0.25 radians per second)
            float rotation = Main.GlobalTimeWrappedHourly * 0.25f;

            // Color (gold with health-based intensity)
            Color drawColor = Color. Lerp(Color.OrangeRed, Color.Gold, healthPercent) * finalAlpha;

            // Slight scale pulse (heartbeat effect)
            float scalePulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.03f + 0.97f;
            float scale = 1.0f * scalePulse;

            // Draw the shield
            DrawData shieldDrawData = new DrawData(
                shieldTexture,
                drawPosition,
                null,
                drawColor,
                rotation,
                shieldTexture.Size() / 2f,
                scale,
                SpriteEffects.None,
                0
            );

            drawInfo.DrawDataCache.Add(shieldDrawData);
            if (healthPercent > 0.5f)
            {
                DrawData glowDrawData = new DrawData(
                    shieldTexture,
                    drawPosition,
                    null,
                    drawColor * 0.3f,
                    rotation,
                    shieldTexture.Size() / 2f,
                    scale * 1.1f,
                    SpriteEffects.None,
                    0
                );

                drawInfo.DrawDataCache.Add(glowDrawData);
            }
        }
    }
}