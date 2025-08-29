using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Buffs
{
    public class WinterGrace : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lavaImmune = true;
            player.fireWalk = true;
            player.ignoreWater = true;
            player.iceSkate = true;
        }
    }
}
