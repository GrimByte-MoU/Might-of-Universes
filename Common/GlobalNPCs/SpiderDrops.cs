using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class SpiderDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.WallCreeper || npc.type == NPCID.BlackRecluse || npc.type == NPCID.WallCreeperWall || npc.type == NPCID.JungleCreeper)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<SpiderSilk>(), 1, false, 0, false, false);
            }
        }
    }
}
