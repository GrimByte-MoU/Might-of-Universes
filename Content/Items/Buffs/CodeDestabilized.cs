using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Buffs
{
    public class CodeDestabilized : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 100;

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.GreenFairy, 0f, 0f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            Lighting.AddLight(npc.Center, 0f, 1f, 0.1f);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 30;

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.GreenFairy, 0f, 0f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            Lighting.AddLight(player.Center, 0f, 1f, 0.1f);
        }
    }
}