using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalTiles
{
    public class AegonArenaTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            // Only restrict breaking if WorldAegis is present and its arena is active
            if (!NPC.AnyNPCs(ModContent.NPCType<Content.NPCs.Bosses.Aegon.WorldAegis>()))
                return true;

            var arena = Content.NPCs.Bosses.Aegon.AegonArena.Current;
            if (arena != null && arena.IsActive && !arena.CanBreakTile(i, j))
            {
                return false; // Prevent breaking this tile
            }

            return true;
        }
    }
}