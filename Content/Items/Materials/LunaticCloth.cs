using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace MightofUniverses.Content.Items.Materials
{
    public class LunaticCloth : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Purple;
            Item.material = true;
        }
    }

    // Add this class to handle the drop from Lunatic Cultist
    public class LunaticClothDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.CultistBoss)
            {
                // Add Lunatic Cloth drop (50-75)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunaticCloth>(), 1, 50, 76));
            }
        }
    }
}
