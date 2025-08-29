using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class ShopAdditions : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Steampunker)
            {
                if (NPC.downedPlantBoss)
                {
                    shop.Add(ModContent.ItemType<BrassBar>(), Condition.TimeDay);
                }
            }
        }
    }
}
