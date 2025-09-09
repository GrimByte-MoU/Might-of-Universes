using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using MightofUniverses.Common.DropConditions;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Common.GlobalItems
{
    public sealed class PlanteraCrateLoot : GlobalItem
    {
        public override bool InstancePerEntity => false;

        public override void ModifyItemLoot(Terraria.Item item, ItemLoot itemLoot)
        {
            var postPlantera = new DownedPlantBossCondition();

            // Hallow (HM-only)
            if (CrateIds.Is(item.type, CrateIds.Hallowed))
                itemLoot.Add(ItemDropRule.ByCondition(postPlantera, ModContent.ItemType<HallowedLight>(), 10, 1, 3));

            // Desert HM: Mirage
            if (CrateIds.Is(item.type, CrateIds.Mirage))
                itemLoot.Add(ItemDropRule.ByCondition(postPlantera, ModContent.ItemType<EclipseLight>(), 10, 1, 3));

            // Snow HM: Boreal
            if (CrateIds.Is(item.type, CrateIds.Boreal))
                itemLoot.Add(ItemDropRule.ByCondition(postPlantera, ModContent.ItemType<FestiveSpirit>(), 10, 1, 3));

            // Sky HM: Azure
            if (CrateIds.Is(item.type, CrateIds.Azure))
                itemLoot.Add(ItemDropRule.ByCondition(postPlantera, ModContent.ItemType<FrozenFragment>(), 10, 1, 3));

            // Ocean HM: Seaside
            if (CrateIds.Is(item.type, CrateIds.Seaside))
                itemLoot.Add(ItemDropRule.ByCondition(postPlantera, ModContent.ItemType<GreedySpirit>(), 10, 1, 3));

            // Corruption HM
            if (CrateIds.Is(item.type, CrateIds.Defiled))
                itemLoot.Add(ItemDropRule.ByCondition(postPlantera, ModContent.ItemType<PureTerror>(), 10, 1, 3));

            // Crimson HM
            if (CrateIds.Is(item.type, CrateIds.Hematic))
                itemLoot.Add(ItemDropRule.ByCondition(postPlantera, ModContent.ItemType<SanguineEssence>(), 10, 1, 3));
        }
    }
}