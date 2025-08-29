using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace MightofUniverses.Common.Players
{
    public class FleshGolemPlayer : ModPlayer
{
    public bool hasFleshGolem;
    public bool hasPrismaticGolem;

    public override void ResetEffects()
    {
        hasFleshGolem = false;
        hasPrismaticGolem = false;
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (hasFleshGolem || hasPrismaticGolem)
        {
            if (Player.HasBuff(ModContent.BuffType<AlteredPerception>()))
                return;

            if (Main.rand.NextFloat() < 0.15f)
            {
                Player.AddBuff(ModContent.BuffType<AlteredPerception>(), hasPrismaticGolem ? 480 : 480);

                float radius = hasPrismaticGolem ? 800f : 560f;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Player.Center) <= radius)
                    {
                        npc.AddBuff(BuffID.Ichor, 180);
                        if (hasPrismaticGolem)
                            npc.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
                        else
                            npc.AddBuff(ModContent.BuffType<Spineless>(), 180);
                    }
                }
                modifiers.FinalDamage *= 0;
            }
        }
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (Player.HasBuff(ModContent.BuffType<AlteredPerception>()))
        {
            if (hasPrismaticGolem)
                modifiers.CritDamage += 0.75f;
            else if (hasFleshGolem)
                modifiers.CritDamage += 0.50f;
        }
    }
}
}