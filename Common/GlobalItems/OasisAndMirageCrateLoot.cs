using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using MightofUniverses.Common.DropConditions;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalItems
{
    public sealed class OasisAndMirageCrateLoot : GlobalItem
    {
        public override bool InstancePerEntity => false;

        public override void ModifyItemLoot(Terraria.Item item, ItemLoot itemLoot)
        {
            var postEvil = new DownedEvilBossCondition();

            if (item.type == ItemID.OasisCrate)
            {
                itemLoot.Add(ItemDropRule.ByCondition(postEvil, ModContent.ItemType<DesertWrappings>(), 10, 3, 5));
                itemLoot.Add(ItemDropRule.ByCondition(postEvil, ModContent.ItemType<SunSigil>(),        10, 1, 2));
                itemLoot.Add(ItemDropRule.ByCondition(postEvil, ModContent.ItemType<MoonSigil>(),       10, 1, 2));
                itemLoot.Add(ItemDropRule.ByCondition(postEvil, ModContent.ItemType<SolunarToken>(),    20, 1, 3));
            }

            if (item.type == ItemID.OasisCrateHard)
            {
                itemLoot.Add(ItemDropRule.ByCondition(postEvil, ModContent.ItemType<DesertWrappings>(), 10, 5, 10));
                itemLoot.Add(ItemDropRule.ByCondition(postEvil, ModContent.ItemType<SunSigil>(),        10, 2, 6));
                itemLoot.Add(ItemDropRule.ByCondition(postEvil, ModContent.ItemType<MoonSigil>(),       10, 2, 6));
                itemLoot.Add(ItemDropRule.ByCondition(postEvil, ModContent.ItemType<SolunarToken>(),    20, 5, 10));
            }
        }
    }
}