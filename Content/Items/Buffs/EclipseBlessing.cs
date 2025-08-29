using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Buffs
{
    public class EclipseBlessing : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = false;
        }


        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.20f;
            player.wingTimeMax = (int)(player.wingTimeMax * 1.4f);
            Lighting.AddLight(player.Center, Color.Orange.ToVector3() * 2.0f); // Strong glow
        }
    }
}
