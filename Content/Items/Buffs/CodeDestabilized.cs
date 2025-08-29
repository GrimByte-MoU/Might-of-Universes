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
            Main.debuff[Type] = true; // It’s a debuff
            Main.buffNoSave[Type] = true; // Not saved on exit
            Main.pvpBuff[Type] = true; // Affects players in PvP
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            // Inflict 100 damage per second (approx. 2 per tick)
            npc.lifeRegen -= 200;

            // Emit neon green glow particles
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GreenFairy);
                Main.dust[dust].scale = 1.2f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }

            Lighting.AddLight(npc.Center, 0f, 1f, 0.1f); // Neon green glow
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Optional: if you ever want players to be affected, apply the same glow
            player.lifeRegen -= 200;

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.GreenFairy);
                Main.dust[dust].scale = 1.2f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }

            Lighting.AddLight(player.Center, 0f, 1f, 0.1f); // Neon green glow
        }
    }
}
