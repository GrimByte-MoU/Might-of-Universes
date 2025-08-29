using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class AccessoryPlayer : ModPlayer
    {
        public bool accessoryModeActive = false;

        public int idleTimer = 0;
        public int bonusStacks = 0;

        public int maxStacks = 6;
        public int framesPerStack = 600;
        public int idleDelay = 300;
        public int defensePerStack = 1;
        public int lifeRegenPerStack = 1;

        public override void ResetEffects()
        {
            accessoryModeActive = false;
            maxStacks = 6;
            framesPerStack = 600;
            idleDelay = 300;
            defensePerStack = 1;
            lifeRegenPerStack = 1;
        }

        public override void PostUpdate()
        {
            if (!accessoryModeActive)
            {
                idleTimer = 0;
                bonusStacks = 0;
                return;
            }

            if (Player.HeldItem.damage > 0 && Player.itemAnimation > 0)
            {
                idleTimer = 0;
                bonusStacks = 0;
                return;
            }

            idleTimer++;

            if (idleTimer >= idleDelay)
            {
                if (idleTimer % framesPerStack == 0 && bonusStacks < maxStacks)
                {
                    bonusStacks++;
                }

                Player.statDefense += bonusStacks * defensePerStack;
                Player.lifeRegen += bonusStacks * lifeRegenPerStack;
            }
        }
    }
}
