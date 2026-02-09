using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Systems
{
    public class AccessoryLootSystem : ModSystem
    {
        #if TML_144
                public override void ModifyFishingLoot(FishingLoot loot)
                {
                    // Monkey's Paw: evil biomes anytime
                    loot.Add(ItemDropRule.ByCondition(new EvilBiomeFishingCondition(),
                        ModContent.ItemType<MonkeysPaw>(), 180)); // 1/180 (tune)
        
                    // Charm of the Depths: Ocean post-Skeletron
                    loot.Add(ItemDropRule.ByCondition(new OceanPostSkeletronFishingCondition(),
                        ModContent.ItemType<CharmOfTheDepths>(), 160));
                }
        #else
                // Fishing loot modifications require tModLoader 1.4.4+. Upgrade tModLoader and define TML_144 to enable this feature.
        #endif

        #if TML_144
                public override void ModifyGlobalLoot(GlobalLoot globalLoot)
                {
                    // Left intentionally blank here; enemy drop handled by GlobalNPC below.
                }
        #endif
    }

    // -------------------------
    // Fishing Conditions
    // -------------------------
    public class EvilBiomeFishingCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
            => info.player.ZoneCrimson || info.player.ZoneCorrupt;

        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Fished in an evil biome";
    }

    public class OceanPostSkeletronFishingCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
            => NPC.downedBoss3 && info.player.ZoneBeach;

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => "Fished in the Ocean after defeating Skeletron";
    }
    public class AccessoryGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.SpawnedFromStatue)
                return;

            if (IsSkeletonTarget(npc.type) && Main.rand.NextFloat() < 0.05f)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(),
                    ModContent.ItemType<SoulSiphoningArtifact>());
            }
        }

        private bool IsSkeletonTarget(int type) =>
            type == NPCID.Skeleton ||
            type == NPCID.UndeadMiner ||
            type == NPCID.Tim ||
            type == NPCID.SkeletonArcher ||
            type == NPCID.ArmoredSkeleton ||
            type == NPCID.RuneWizard;
    }

    // -------------------------
    // Skeleton Merchant shop additions
    // -------------------------
    public class SkeletonMerchantShop : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType != NPCID.SkeletonMerchant)
                return;

            if (NPC.downedBoss1)
                shop.Add(ModContent.ItemType<FerrymansToken>());

            if (NPC.downedBoss2)
                shop.Add(ModContent.ItemType<GravediggersRing>());
        }
    }
}