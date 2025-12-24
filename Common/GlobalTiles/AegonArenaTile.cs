// Common/GlobalTiles/AegonArenaTile.cs

using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalTiles
{
    public class AegonArenaTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            // Check if Aegon is active
            if (! NPC.AnyNPCs(ModContent.NPCType<Content.NPCs.Bosses.Aegon.Aegon>()))
                return true;

            // Find Aegon
            for (int n = 0; n < Main. maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active && npc.type == ModContent.NPCType<Content.NPCs.Bosses.Aegon.Aegon>())
                {
                    // Get arena instance from Aegon
                    var aegon = npc.ModNPC as Content.NPCs.Bosses.Aegon.Aegon;
                    if (aegon?. Arena != null && aegon.Arena.IsActive)
                    {
                        if (! aegon.Arena.CanBreakTile(i, j))
                        {
                            return false; // Prevent breaking
                        }
                    }
                }
            }

            return true;
        }
    }
}