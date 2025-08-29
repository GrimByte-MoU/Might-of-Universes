using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Buffs
{
    public class Terrified : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<TerrifiedNPC>().isTerrified = true;
        }
    }

    public class TerrifiedNPC : GlobalNPC
    {
        public bool isTerrified;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            isTerrified = false;
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (isTerrified)
                modifiers.FinalDamage *= 1.2f;
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile proj, ref NPC.HitModifiers modifiers)
        {
            if (isTerrified)
                modifiers.FinalDamage *= 1.2f;
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (isTerrified)
                modifiers.IncomingDamageMultiplier *= 0.8f;
        }
    }
}
