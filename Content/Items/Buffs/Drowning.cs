using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;

namespace MightofUniverses.Content.Items.Buffs
{
    public class Drowning : ModBuff
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
            if (Main.GameUpdateCount % 60 == 0) // Once per second (60 ticks)
            {
                player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(NetworkText.FromKey(player.name + "drowned.")), 75, 0);
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.WaterCandle, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
                dust.alpha = 150;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.GameUpdateCount % 60 == 0) // Once per second (60 ticks)
            {
                npc.StrikeNPC(new NPC.HitInfo { Damage = 75 });
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.WaterCandle, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
                dust.alpha = 150;
            }
        }
    }
}
