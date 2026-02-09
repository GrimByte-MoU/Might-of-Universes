using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class FrostMoonDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.IceQueen)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<FestiveSpirit>(), Main.rand.Next(3, 10));
            }

            if (npc.type == NPCID.SantaNK1 || npc.type == NPCID.Everscream)
            {
                if (Main.rand.NextFloat() < .75f)
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<FestiveSpirit>(), Main.rand.Next(2, 8));
                }
            }

            if (npc.type == NPCID.Krampus || npc.type == NPCID.Flocko)
            {
                if (Main.rand.NextFloat() < .50f)
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<FestiveSpirit>(), Main.rand.Next(1, 4));
                }
            }
        }
    }
}
