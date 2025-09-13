using System;
using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Buffs
{
    public class GuidingLight : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // 1) Damage Reduction: +4% endurance
            player.endurance += 0.04f;

            // 2) Scaled regeneration: +1% of max life per second
            // lifeRegen is in half-HP/sec units (2 = +1 HP/s)
            float hpPerSecond = player.statLifeMax2 * 0.01f; // 1% of max life per second
            int lifeRegenAdd = (int)Math.Round(hpPerSecond * 2f);
            player.lifeRegen += lifeRegenAdd;
        }
    }
}