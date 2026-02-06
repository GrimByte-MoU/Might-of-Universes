using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.GlobalNPCs;

namespace MightofUniverses.Content.Items.Buffs
{
    public class SheerCold : ModBuff
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
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.IceTorch, 0f, 0f, 100, Color.LightBlue, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.fadeIn = 1.3f;
                
                if (Main.rand.NextBool(4))
                {
                    float angle = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Vector2 offset = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * 25f;
                    Dust iceDust = Dust.NewDustPerfect(npc.Center + offset, DustID.IceTorch, Vector2.Zero, 100, Color.Cyan, 2.0f);
                    iceDust.noGravity = true;
                }
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 50;
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.IceTorch, 0f, 0f, 100, Color.LightBlue, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }
        }
    }
}