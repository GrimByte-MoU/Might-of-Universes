using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class MoonLordDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.LunarOre, 1, 130, 210));
                npcLoot.Add(ItemDropRule.Common(ItemID.FragmentSolar, 1, 75, 125));
                npcLoot.Add(ItemDropRule.Common(ItemID.FragmentNebula, 1, 75, 125));
                npcLoot.Add(ItemDropRule.Common(ItemID.FragmentStardust, 1, 75, 125));
                npcLoot.Add(ItemDropRule.Common(ItemID.FragmentVortex, 1, 75, 125));
            }
        }
    }
}
