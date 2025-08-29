using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;

namespace MightofUniverses.Content.Items.Buffs
{
    public class LordsVenom : ModBuff
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
                player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(NetworkText.FromKey(player.name + " was dissolved by Lord's Venom.")), 75, 0);
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Poisoned, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.GameUpdateCount % 6 == 0)
            {
                npc.StrikeNPC(new NPC.HitInfo { Damage = 100 });
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Poisoned, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }
    }
}
