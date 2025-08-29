using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MightofUniverses.Common.UI;

namespace MightofUniverses.Common.Systems
{
    public class ReaperUISystem : ModSystem
    {
        internal static UserInterface ReaperInterface;
        internal static ReaperUI ReaperUIState;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                ReaperUIState = new ReaperUI();
                ReaperInterface = new UserInterface();
                ReaperInterface.SetState(ReaperUIState);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (ReaperInterface?.CurrentState != null)
            {
                ReaperInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));

            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex + 1, new LegacyGameInterfaceLayer(
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
}
