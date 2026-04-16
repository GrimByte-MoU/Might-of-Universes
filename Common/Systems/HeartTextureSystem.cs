using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.UI;

namespace MightofUniverses.Common.Systems
{
    public class HeartTextureSystem : ModSystem
    {
        private static Asset<Texture2D> heart3Asset;

        private const int GoldLife = 500;
        private const int LifePerHeart = 20;
        private const int HeartsPerRow = 10;
        private const int HeartSpacingX = 26;
        private const int HeartSpacingY = 32;
        private const int HeartStartX = 16;
        private const int HeartStartY = 32;

        public override void Load()
        {
            if (Main.dedServ)
                return;

            heart3Asset = ModContent.Request<Texture2D>(
                "MightofUniverses/Content/Textures/Heart3",
                AssetRequestMode.ImmediateLoad
            );
        }

        public override void Unload()
        {
            heart3Asset = null;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarsIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Health / Mana Bars"));

            if (resourceBarsIndex == -1)
                return;

            layers.Insert(resourceBarsIndex + 1, new LegacyGameInterfaceLayer(
                "MightofUniverses: Silver Hearts",
                delegate
                {
                    DrawSilverHearts();
                    return true;
                },
                InterfaceScaleType.UI
            ));
        }

        private static void DrawSilverHearts()
        {
            if (Main.LocalPlayer == null || !Main.LocalPlayer.active)
                return;

            Player player = Main.LocalPlayer;

            int totalHearts = player.statLifeMax2 / LifePerHeart;
            int silverHeartStart = GoldLife / LifePerHeart;

            if (totalHearts <= silverHeartStart)
                return;

            Texture2D heart3Tex = heart3Asset.Value;
            int currentLife = player.statLife;

            for (int i = silverHeartStart; i < totalHearts; i++)
            {
                int col = i % HeartsPerRow;
                int row = i / HeartsPerRow;

                float x = HeartStartX + col * HeartSpacingX;
                float y = HeartStartY + row * HeartSpacingY;

                int heartLifeMin = i * LifePerHeart;
                int heartLifeMax = heartLifeMin + LifePerHeart;

                bool isFull = currentLife >= heartLifeMax;
                bool isEmpty = currentLife <= heartLifeMin;

                if (isFull)
                {
                    Main.spriteBatch.Draw(heart3Tex, new Vector2(x, y), Color.White);
                }
                else if (isEmpty)
                {
                    Main.spriteBatch.Draw(heart3Tex, new Vector2(x, y), Color.Gray * 0.5f);
                }
                else
                {
                    int lifeInThisHeart = currentLife - heartLifeMin;
                    int partialWidth = (int)(heart3Tex.Width * (lifeInThisHeart / (float)LifePerHeart));

                    Main.spriteBatch.Draw(
                        heart3Tex,
                        new Vector2(x, y),
                        new Rectangle(0, 0, partialWidth, heart3Tex.Height),
                        Color.White
                    );

                    Main.spriteBatch.Draw(
                        heart3Tex,
                        new Vector2(x + partialWidth, y),
                        new Rectangle(partialWidth, 0, heart3Tex.Width - partialWidth, heart3Tex.Height),
                        Color.Gray * 0.5f
                    );
                }
            }
        }
    }
}