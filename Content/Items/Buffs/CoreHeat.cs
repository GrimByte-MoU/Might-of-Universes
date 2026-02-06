using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Buffs
{
    public class CoreHeat : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 800;
            
            if (Main.rand.NextBool(2))
            {
                int dustChoice = Main.rand.Next(3);
                int dustType = dustChoice == 0 ? DustID.Torch : (dustChoice == 1 ? DustID.Smoke : DustID.Ash);
                
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, dustType, 0f, -2f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity.Y -= 1.5f;
                dust.fadeIn = 1.3f;
                
                if (Main.rand.NextBool(3))
                {
                    Dust ember = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.FlameBurst, 0f, 0f, 100, Color.OrangeRed, 2.0f);
                    ember.noGravity = true;
                    ember.velocity = Main.rand.NextVector2Circular(2f, 2f);
                }
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 50;
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Torch, 0f, -1f, 100, default, 1.5f);
                dust.noGravity = true;
            }
        }
    }
}