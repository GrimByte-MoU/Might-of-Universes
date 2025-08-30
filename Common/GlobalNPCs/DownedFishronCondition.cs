using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace MightofUniverses.Common.DropConditions
{
    public sealed class DownedFishronCondition : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => NPC.downedFishron;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "after Duke Fishron has been defeated";
    }
}