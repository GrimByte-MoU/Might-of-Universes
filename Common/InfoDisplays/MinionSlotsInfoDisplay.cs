using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.InfoDisplays
{
    // Custom InfoDisplay that shows "Minions: X" based on Player.maxMinions.
    // This display is active when the player effectively "has" a Tabulating Machine
    // (equipped, in inventory, or in any personal bank), as flagged by TabulatingMachinePlayer.
    public class MinionSlotsInfoDisplay : InfoDisplay
    {
        public override string Texture => "MightofUniverses/Assets/InfoDisplays/MinionSlotsInfoDisplay";

        // Whether this info display should be shown
        public override bool Active()
        {
            Player player = Main.LocalPlayer;
            return player.GetModPlayer<TabulatingMachinePlayer>().HasTabulatingMachine;
        }

        // The text shown next to the icon
        public override string DisplayValue(ref Color displayColor)
        {
            Player player = Main.LocalPlayer;
            int minions = player.maxMinions;

            // Vanilla info colors often use white unless context demands otherwise.
            displayColor = Color.White;
            return $"Minions: {minions}";
        }
    }
}