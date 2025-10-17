using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Buffs
{
    public class OceanPressure : ModBuff
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

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= (int)(player.statDefense * 0.5f);
            player.jumpSpeedBoost *= 0.5f;
            player.jumpBoost = false;
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Water, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 800;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Water, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }
    }
}
