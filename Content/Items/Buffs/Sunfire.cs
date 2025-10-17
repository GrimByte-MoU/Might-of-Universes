using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Buffs
{
    public class Sunfire : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 1500;

            Lighting.AddLight(npc.Center, 1.0f, 0.6f, 0.0f);

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.fadeIn = 1.2f;

                if (Main.rand.NextBool(3))
                {
                    Dust dust2 = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.SolarFlare, 0f, 0f, 100, default, 1.0f);
                    dust2.noGravity = true;
                    dust2.velocity *= 0.7f;
                    dust2.fadeIn = 1.5f;
                }
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 150;

            Lighting.AddLight(player.Center, 1.0f, 0.6f, 0.0f);

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.fadeIn = 1.2f;

                if (Main.rand.NextBool(3))
                {
                    Dust dust2 = Dust.NewDustDirect(player.position, player.width, player.height, DustID.SolarFlare, 0f, 0f, 100, default, 1.0f);
                    dust2.noGravity = true;
                    dust2.velocity *= 0.7f;
                    dust2.fadeIn = 1.5f;
                }
            }
        }
    }
}