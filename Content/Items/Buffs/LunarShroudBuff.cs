using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Buffs
{
    public class LunarShroudBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statLifeMax2 += 100;
            player.lifeRegen += 10;
            player.statDefense += 8;
            player.moveSpeed += 0.30f;
        }
    }
}