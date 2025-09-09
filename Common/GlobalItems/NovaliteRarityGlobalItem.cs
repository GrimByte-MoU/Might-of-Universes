using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using Terraria.Utilities;
using MightofUniverses.Content.Rarities;
using Terraria.ID;
using Terraria.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace MightofUniverses.Common.GlobalItems
{
    public class NovaliteRarityGlobalItem : GlobalItem
    {
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (item.rare != ModContent.GetInstance<NovaliteRarity>().Type || !(line.Mod == "Terraria" && line.Name == "ItemName"))
                return true;

            Vector2 pos = new(line.X, line.Y);
            Vector2 fontSize = FontAssets.MouseText.Value.MeasureString(line.Text);
            UnifiedRandom rand = new(Main.LocalPlayer.name.GetHashCode() + (int)(fontSize.X + fontSize.Y));
            
            int sparkleCount = rand.Next((int)fontSize.X / 8, (int)fontSize.X / 6) + 1;
            DrawStarEffect(sparkleCount, pos, fontSize, ModContent.GetInstance<NovaliteRarity>().RarityColor, rand);
            
            return true;
        }

        private void DrawStarEffect(int sparkleCount, Vector2 pos, Vector2 fontSize, Color baseColor, UnifiedRandom rand)
        {
            Asset<Texture2D> starTexture = ModContent.Request<Texture2D>("Terraria/Images/Star");
            Vector2 starOrigin = new Vector2(starTexture.Width(), starTexture.Height()) / 2;

            for (int i = 0; i < sparkleCount; i++)
            {
                Color color = baseColor;
                Color color2 = color * 0.75f;
                Vector2 v = new(rand.NextFloat(fontSize.X), rand.NextFloat(fontSize.Y * 0.8f));
                float lifeTime = Main.GlobalTimeWrappedHourly * 5.6f + rand.NextFloat((float)Math.PI * 14f);
                lifeTime %= (float)Math.PI * 14f;
                float starRotation = rand.NextFloat(0, MathHelper.TwoPi);

                if (lifeTime <= (float)Math.PI * 2f)
                {
                    float sinValue = (float)Math.Sin(lifeTime);
                    Color white = (new Color(200 + color.R / 20, 200 + color.G / 20, 200 + color.B / 20, 255) * sinValue);
                    
                    Main.spriteBatch.Draw(starTexture.Value, new Vector2(pos.X, pos.Y - lifeTime * 1f + 2f) + v, null, white, starRotation, starOrigin, (lifeTime / ((float)Math.PI * 2f) * 0.66f) / 2, SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(starTexture.Value, new Vector2(pos.X, pos.Y - lifeTime * 1f + 2f) + v, null, white * 0.5f, starRotation, starOrigin, (lifeTime / ((float)Math.PI * 2f)) / 2, SpriteEffects.None, 0f);
                    
                    float scale3 = lifeTime / ((float)Math.PI * 2f) * 1.5f;
                    Color col2 = color2 * sinValue;
                    
                    Main.spriteBatch.Draw(starTexture.Value, pos + v, null, col2, starRotation, starOrigin, scale3 * 0.9f, SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(starTexture.Value, pos + v, null, col2, starRotation, starOrigin, scale3 * 0.6f, SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(starTexture.Value, pos + v, null, col2, starRotation, starOrigin, scale3 * 0.55f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}




