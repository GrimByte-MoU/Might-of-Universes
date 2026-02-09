using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class MortalWoundNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool hasMortalWound;

        public override void ResetEffects(NPC npc)
        {
            hasMortalWound = false;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (hasMortalWound)
            {
                modifiers.FinalDamage *= 1.20f;
            }
        }
    }
}
