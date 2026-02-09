using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Common.Players
{
    public class GreedySpiritPlayer : ModPlayer
    {
        public bool hasIncenseOfSuccess = false;

public override void ResetEffects()
{
    hasIncenseOfSuccess = false;
}

public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
{
    if (hasIncenseOfSuccess)
    {
        healValue = (int)(healValue * 1.25f);
    }
}

public override bool OnPickup(Item item)
{
    if (hasIncenseOfSuccess &&
        (item.type == ItemID.CopperCoin || item.type == ItemID.SilverCoin ||
         item.type == ItemID.GoldCoin || item.type == ItemID.PlatinumCoin))
    {
        item.stack = (int)(item.stack * 1.25f);
        return true;
    }
    return true;
}

    }
}
