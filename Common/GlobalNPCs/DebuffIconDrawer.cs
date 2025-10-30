using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class DebuffIconDrawer : GlobalNPC
    {
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!npc.active || npc.life <= 0) return;

            DrawBuffIcons(npc, spriteBatch, screenPos);
        }

        private void DrawBuffIcons(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos)
        {
            Vector2 position = npc.Top - Main.screenPosition + new Vector2(npc.width / 2f, -20f);

            const float iconSize = 16f;
            const float spacing = 2f;
            const int iconsPerRow = 5;

            var debuffs = new System.Collections.Generic.List<int>();
            for (int i = 0; i < NPC.maxBuffs; i++)
            {
                if (npc.buffType[i] > 0 && npc.buffTime[i] > 0 && Main.debuff[npc.buffType[i]])
                {
                    debuffs.Add(npc.buffType[i]);
                }
            }

            if (debuffs.Count == 0) return;

            int rows = (debuffs.Count + iconsPerRow - 1) / iconsPerRow;
            float rowWidth = iconsPerRow * iconSize + (iconsPerRow - 1) * spacing;
            position.X -= rowWidth / 2f;

            for (int row = 0; row < rows; row++)
            {
                float rowY = position.Y + row * (iconSize + spacing);
                int iconsInThisRow = System.Math.Min(iconsPerRow, debuffs.Count - row * iconsPerRow);

                for (int col = 0; col < iconsInThisRow; col++)
                {
                    int buffType = debuffs[row * iconsPerRow + col];
                    Texture2D texture = TextureAssets.Buff[buffType].Value;
                    if (texture != null)
                    {
                        Rectangle frame = texture.Frame(1, 1, 0, 0);
                        Vector2 iconPos = new Vector2(position.X + col * (iconSize + spacing), rowY);
                        spriteBatch.Draw(texture, iconPos, frame, Color.White, 0f, frame.Size() / 2f, iconSize / frame.Width, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}