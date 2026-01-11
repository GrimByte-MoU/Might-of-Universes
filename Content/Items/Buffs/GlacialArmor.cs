using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Buffs
{
    public class GlacialArmor : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 10;
            player. endurance += 0.05f;
            player.noKnockback = true;
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Ice, 0f, 0f, 100, Color. Cyan, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.fadeIn = 1.0f;
            }
        }
    }
}