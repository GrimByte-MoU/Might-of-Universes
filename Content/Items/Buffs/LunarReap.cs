using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses. Content.Items. Buffs
{
    public class LunarReap : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID. Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.debuff[Type] = true;
            Main. pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // BALANCED: -16 lifeRegen = -8 HP/s (same as Cursed Inferno)
            player.lifeRegen -= 16;
            
            // BALANCED: -10% endurance (reduced from -20%)
            player. endurance -= 0.10f;
            
            // BALANCED:  -12% damage output (reduced from -25%)
            player.GetDamage(DamageClass.Generic) -= 0.12f;

            // Visual effect (less frequent to reduce lag)
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 
                    DustID.BlueTorch, 0f, 0f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.fadeIn = 0.3f;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            // BRUTAL: -400 lifeRegen = -200 HP/s for NPCs
            npc.lifeRegen -= 400;
            
            // Add dramatic visual effect for NPCs
            if (Main. rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc. height, 
                    DustID.BlueTorch, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity = npc.velocity * 0.2f;
                dust.fadeIn = 0.4f;
                dust.scale = 1.3f;
            }
        }
    }
}