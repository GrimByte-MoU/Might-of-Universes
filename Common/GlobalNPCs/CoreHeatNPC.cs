using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class CoreHeatNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int heatStacks = 0;
        private const int maxStacks = 10;

        public override void ResetEffects(NPC npc)
        {
            if (!npc.HasBuff<CoreHeat>())
            {
                heatStacks = 0;
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff<CoreHeat>())
            {
                if (heatStacks < maxStacks)
                {
                    heatStacks++;
                }

                float damageBonus = 0.1f + (heatStacks * 0.02f);
                modifiers.FinalDamage += damageBonus;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff<CoreHeat>())
            {
                if (heatStacks < maxStacks)
                {
                    heatStacks++;
                }

                float damageBonus = 0.1f + (heatStacks * 0.02f);
                modifiers.FinalDamage += damageBonus;
            }
        }
    }
}