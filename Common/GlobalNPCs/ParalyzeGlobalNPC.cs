using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class ParalyzeGlobalNPC : GlobalNPC
    {
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            int paralyze = ModContent.BuffType<Paralyze>();
            if (npc.active && npc.HasBuff(paralyze))
            {
                drawColor = new Color(80, 160, 255);
            }
        }
    }
}