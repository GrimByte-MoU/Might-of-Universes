using System;
using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses. Content.Items.Buffs
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
            player.endurance += 0.04f;
            float hpPerSecond = player.statLifeMax2 * 0.01f;
            int lifeRegenAdd = (int)Math.Round(hpPerSecond * 2f);
            player.lifeRegen += lifeRegenAdd;
        }
    }
}