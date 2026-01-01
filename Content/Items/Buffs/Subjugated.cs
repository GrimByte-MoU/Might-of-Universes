using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Buffs
{
    public class Subjugated : ModBuff
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
            npc.damage = (int)(npc.damage * 0.75f);
            
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Wraith, 0f, 0f, 100, Color.Gray, 1.0f);
                dust.noGravity = true;
                dust.fadeIn = 0.5f;
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity.X *= 0.5f;
            
            if (!player.justJumped && !player.sliding)
            {
                player.velocity.Y *= 0.5f;
            }
            
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Wraith, 0f, 0f, 100, Color.Gray, 1.0f);
                dust.noGravity = true;
                dust.fadeIn = 0.5f;
            }
        }
    }
}