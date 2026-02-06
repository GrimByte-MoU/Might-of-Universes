using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Buffs
{
    public class NaturesToxin : ModBuff
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
            
            if (!npc.boss)
            {
                int actualDamagePerSecond = 400;
                int reduction = actualDamagePerSecond / 60;
                
                if (npc.lifeMax > reduction)
                {
                    npc.lifeMax -= reduction;
                    
                    if (npc.life > npc.lifeMax)
                    {
                        npc.life = npc.lifeMax;
                    }
                }
            }
            
            if (Main.rand.NextBool(2))
            {
                int dustChoice = Main.rand.Next(3);
                int dustType = dustChoice == 0 ? DustID.JungleSpore : (dustChoice == 1 ? DustID.Poisoned : DustID.JunglePlants);
                
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, dustType, 0f, 0f, 100, Color.LimeGreen, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.fadeIn = 1.3f;
                
                if (Main.rand.NextBool(4))
                {
                    Dust venom = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Venom, 0f, 0f, 100, Color.Green, 2.0f);
                    venom.noGravity = true;
                    venom.velocity.Y -= 2f;
                }
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 50;
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Poisoned, 0f, 0f, 100, Color.Green, 1.5f);
                dust.noGravity = true;
            }
        }
    }
}