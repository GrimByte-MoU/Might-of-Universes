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
            // Bramble Crate = HM Jungle crate
            if (item.type == ItemID.JungleFishingCrateHard)
            {
                var postFishron = new DownedFishronCondition();
                // "Rarely": 1 in 10 chance each; stack 1–3.
                itemLoot.Add(ItemDropRule.ByCondition(postFishron, ModContent.ItemType<PrehistoricAmber>(), chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));
                itemLoot.Add(ItemDropRule.ByCondition(postFishron, ModContent.ItemType<AncientBone>(),       chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));
                itemLoot.Add(ItemDropRule.ByCondition(postFishron, ModContent.ItemType<TarChunk>(),          chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));
            }

            // Expert/Master treasure bag (FishronBossBag) bonus bones
            if (item.type == ItemID.FishronBossBag)
            {
                // Always rolls inside the bag: 15–20 Ancient Bone
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AncientBone>(), chanceDenominator: 1, minimumDropped: 15, maximumDropped: 20));
            }
        }
    }
}