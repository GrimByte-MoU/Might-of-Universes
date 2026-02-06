using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Buffs
{
    public class DeltaShock : ModBuff
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
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.fadeIn = 1.3f;
                
                if (Main.rand.NextBool(3))
                {
                    Vector2 sparkVelocity = Main.rand.NextVector2Circular(3f, 3f);
                    Dust spark = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Electric, sparkVelocity.X, sparkVelocity.Y, 100, Color.Yellow, 2.0f);
                    spark.noGravity = true;
                }
            }
            
            if (Main.rand.NextBool(60))
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 position = npc.Center + Main.rand.NextVector2Circular(npc.width, npc.height);
                    Dust lightning = Dust.NewDustPerfect(position, DustID.Electric, Vector2.Zero, 100, Color.White, 2.5f);
                    lightning.noGravity = true;
                }
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 50;
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 1.5f);
                dust.noGravity = true;
            }
        }
    }
}