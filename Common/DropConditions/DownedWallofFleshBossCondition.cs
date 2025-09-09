using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace MightofUniverses.Common.DropConditions
{
    public sealed class DownedWallofFleshCondition : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => Main.hardMode;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "after Wall of Flesh has been defeated";
    }
}