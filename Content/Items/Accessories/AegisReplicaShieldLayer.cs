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

            Texture2D shieldTexture = ModContent.Request<Texture2D>(
                "MightofUniverses/Content/Items/Accessories/AegisReplicaShield",
                AssetRequestMode.ImmediateLoad
            ).Value;

            Vector2 drawPosition = player.MountedCenter - Main.screenPosition;
            drawPosition = new Vector2((int)drawPosition.X, (int)drawPosition.Y);

            float healthPercent = modPlayer.shieldHealth / modPlayer.maxShieldHealth;

            float baseAlpha = healthPercent;

            float finalAlpha = baseAlpha;

            float rotation = Main.GlobalTimeWrappedHourly * 0.25f;

            Color drawColor = Color. Lerp(Color.OrangeRed, Color.Gold, healthPercent) * finalAlpha;

            float scalePulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.03f + 0.97f;
            float scale = 1.0f * scalePulse;

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