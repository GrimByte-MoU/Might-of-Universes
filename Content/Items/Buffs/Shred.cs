using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Buffs
{
    public class Shred : ModBuff
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
    if (Main.GameUpdateCount % 12 == 0)
    {
        npc.StrikeNPC(new NPC.HitInfo { Damage = 35 });
    }
    
    if (Main.rand.NextBool(2))
    {
        Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Dirt, 0f, 0f, 100, default, 0.8f);
        dust.noGravity = true;
        dust.fadeIn = 0.2f;
    }
}

    }
}
