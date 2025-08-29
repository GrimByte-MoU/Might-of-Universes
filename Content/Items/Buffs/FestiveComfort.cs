using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Buffs
{
    public class FestiveComfort : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.enemySpawns = false;
            player.GetModPlayer<FestiveComfortPlayer>().hasFestiveComfort = true;
        }
    }

    public class FestiveComfortPlayer : ModPlayer
    {
        public bool hasFestiveComfort = false;

        public override void ResetEffects()
        {
            hasFestiveComfort = false;
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
{
    if (hasFestiveComfort && IsColdEnemy(npc))
    {
        modifiers.SourceDamage *= 0.7f; // Reduce damage by 30%
    }
}


        private bool IsColdEnemy(NPC npc)
        {
            return npc.type == NPCID.IceElemental || npc.type == NPCID.IceTortoise || npc.type == NPCID.IceSlime || npc.type == NPCID.SnowFlinx || npc.type == NPCID.IceGolem || npc.type == NPCID.IceQueen || npc.type == NPCID.Flocko;
        }
    }
}
