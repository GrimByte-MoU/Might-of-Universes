using Terraria;
using Terraria. ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Buffs
{
    public class TerrasRend : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets. NurseCannotRemoveDebuff[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 60;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    player.position, 
                    player.width, 
                    player.height, 
                    DustID. Grass, 
                    0f, 0f, 100, 
                    default, 
                    0.9f
                );
                dust. noGravity = true;
                dust.fadeIn = 0.3f;
                
                dust = Dust.NewDustDirect(
                    player.position, 
                    player.width, 
                    player.height, 
                    DustID.Stone, 
                    0f, 0f, 100, 
                    Color.SaddleBrown, 
                    0.8f
                );
                dust. noGravity = false;
                dust.velocity. Y += 0.3f;
            }
            
            if (Main.rand.NextBool(20))
            {
                Dust crack = Dust.NewDustDirect(
                    player.position, 
                    player.width, 
                    player.height, 
                    DustID.Stone, 
                    Main.rand.NextFloat(-1f, 1f), 
                    Main.rand.NextFloat(-2f, 0f), 
                    100, 
                    Color.DarkGreen, 
                    1.2f
                );
                crack. noGravity = false;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 600;
            if (Main.rand. NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    npc.position, 
                    npc.width, 
                    npc. height, 
                    DustID.Grass, 
                    0f, 0f, 100, 
                    default, 
                    0.9f
                );
                dust. noGravity = true;
                dust.fadeIn = 0.3f;
                
                dust = Dust.NewDustDirect(
                    npc.position, 
                    npc.width, 
                    npc.height, 
                    DustID.Stone, 
                    0f, 0f, 100, 
                    Color.SaddleBrown, 
                    0.8f
                );
                dust. noGravity = false;
                dust.velocity.Y += 0.3f;
            }
            
            if (Main.rand.NextBool(20))
            {
                Dust crack = Dust.NewDustDirect(
                    npc.position, 
                    npc.width, 
                    npc.height, 
                    DustID.Stone, 
                    Main.rand.NextFloat(-1f, 1f), 
                    Main.rand.NextFloat(-2f, 0f), 
                    100, 
                    Color.DarkGreen, 
                    1.2f
                );
                crack.noGravity = false;
            }
        }
    }
}