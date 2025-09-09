using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace MightofUniverses.Common.DropConditions
{
    public sealed class DownedEvilBossCondition : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => NPC.downedBoss2;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "after Eater of Worlds or Brain of Cthulhu has been defeated";
    }
}