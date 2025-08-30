using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalNPCs
{
    public sealed class FishronDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.DukeFishron)
            {
                // Normal mode (non-Expert/Master) drop: 10–15 Ancient Bone
                var notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientBone>(), chanceDenominator: 1, minimumDropped: 10, maximumDropped: 15));
                npcLoot.Add(notExpert);
            }
        }
    }
}