using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using MightofUniverses.Common.DropConditions;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalItems
{
    public sealed class BrambleCrateAndBossBagLoot : GlobalItem
    {
        public override bool InstancePerEntity => false;

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.JungleFishingCrateHard)
            {
                var postFishron = new DownedFishronCondition();
                itemLoot.Add(ItemDropRule.ByCondition(postFishron, ModContent.ItemType<PrehistoricAmber>(), chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));
                itemLoot.Add(ItemDropRule.ByCondition(postFishron, ModContent.ItemType<AncientBone>(),       chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));
                itemLoot.Add(ItemDropRule.ByCondition(postFishron, ModContent.ItemType<TarChunk>(),          chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));
            }

            if (item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AncientBone>(), chanceDenominator: 1, minimumDropped: 15, maximumDropped: 20));
            }
        }
    }
}