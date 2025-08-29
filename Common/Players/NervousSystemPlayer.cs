using Terraria;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace MightofUniverses.Common.Players
{
    public class NervousSystemPlayer : ModPlayer
    {
        public bool hasNervousSystem;

        public override void ResetEffects()
        {
            hasNervousSystem = false;
        }

public override void ModifyHurt(ref Player.HurtModifiers modifiers)
{
    if (hasNervousSystem)
    {
        if (Player.HasBuff(ModContent.BuffType<CerebralMindtrick>()))
            return;

        if (Main.rand.NextFloat() < 0.15f)
        {
            Player.AddBuff(ModContent.BuffType<CerebralMindtrick>(), 360);
            
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Player.Center) <= 400f)
                {
                    npc.AddBuff(BuffID.Ichor, 180);
                    npc.AddBuff(ModContent.BuffType<Spineless>(), 180);
                }
            }
             modifiers.FinalDamage *= 0;
        }
    }
    
}



    }
}
