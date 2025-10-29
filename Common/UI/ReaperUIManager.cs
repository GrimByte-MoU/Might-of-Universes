using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MightofUniverses.Common.UI
{
    public class ReaperUIManager : ModSystem
    {
        internal UserInterface ReaperInterface;
        internal ReaperUI ReaperUIState;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                ReaperUIState = new ReaperUI();
                ReaperInterface = new UserInterface();
                ReaperInterface.SetState(ReaperUIState);
                Main.NewText("[ReaperUIManager] Reaper UI attached", Color.LightGreen);
            }
        }

        public override void Unload()
        {
            ReaperInterface = null;
            ReaperUIState = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (ReaperInterface?.CurrentState != null)
                ReaperInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarsIndex = layers.FindIndex(layer => layer.Name?.Contains("Vanilla: Resource Bars") ?? false);
            int insertionIndex = resourceBarsIndex >= 0 ? resourceBarsIndex + 1 : layers.Count;

            layers.Insert(insertionIndex, new LegacyGameInterfaceLayer(
                "MightofUniverses: Reaper UI",
                delegate
                {
                    if (ReaperInterface?.CurrentState != null)
                    {
                        ReaperInterface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}