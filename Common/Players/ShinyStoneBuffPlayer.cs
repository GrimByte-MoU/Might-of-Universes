using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Common.Players
{
    public class ShinyStoneBuffPlayer : ModPlayer
    {
        public bool hasShinyStone = false;

        public override void ResetEffects()
        {
            hasShinyStone = false;
        }

        public override void PostUpdateEquips()
        {
            if (hasShinyStone)
            {
                Player.statLifeMax2 += 50;
                Player.moveSpeed -= 0.10f;
                Player.AddBuff(BuffID.Inferno, 2);
            }
        }
    }
}
