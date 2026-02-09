using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using System;

namespace MightofUniverses.Common.Systems
{
    public static class DataZapSystem
    {
        private const float MAX_DISTANCE = 480f;
        private const float ARC_ANGLE = 0.7853982f;

        public static void TryZapEnemies(Player player, Vector2 sourcePos, Vector2 aimDirection, int baseDamage, float knockback)
        {
            NPC firstTarget = FindFirstTarget(player, sourcePos, aimDirection);
            if (firstTarget == null)
                return;
            NPC[] zapChain = new NPC[4];
            float[] damageMult = { 1.2f, 1.0f, 0.75f, 0.5f };

            zapChain[0] = firstTarget;
            for (int i = 1; i < zapChain.Length; i++)
            {
                zapChain[i] = FindNextTarget(zapChain[i - 1], zapChain, i, MAX_DISTANCE);
                if (zapChain[i] == null)
                    break;
            }

            for (int i = 0; i < zapChain.Length; i++)
            {
                if (zapChain[i] != null)
                {
                    int finalDamage = (int)(baseDamage * damageMult[i]);
                    NPC.HitInfo hitInfo = new NPC.HitInfo()
                    {
                        Damage = finalDamage,
                        Knockback = knockback,
                        HitDirection = player.direction
                    };
                    zapChain[i].StrikeNPC(hitInfo);
                    zapChain[i].AddBuff(ModContent.BuffType<CodeDestabilized>(), 60);


                    if (i > 0 && zapChain[i - 1] != null)
                        DrawGreenBeam(zapChain[i - 1].Center, zapChain[i].Center);
                }
            }
        }

        private static NPC FindFirstTarget(Player player, Vector2 origin, Vector2 direction)
        {
            NPC best = null;
            float bestDist = MAX_DISTANCE;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.CanBeChasedBy()) continue;

                Vector2 toNPC = npc.Center - origin;
                float angle = Vector2.Dot(Vector2.Normalize(direction), Vector2.Normalize(toNPC));
                float distance = toNPC.Length();

                if (distance < bestDist && angle > MathF.Cos(ARC_ANGLE / 2))
                {
                    best = npc;
                    bestDist = distance;
                }
            }
            return best;
        }

        private static NPC FindNextTarget(NPC from, NPC[] previousChain, int currentIndex, float range)
        {
            NPC best = null;
            float bestDist = range;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.CanBeChasedBy() || npc.whoAmI == from.whoAmI)
                    continue;

                // Skip already hit
                bool alreadyZapped = false;
                for (int i = 0; i < currentIndex; i++)
                    if (previousChain[i] != null && previousChain[i].whoAmI == npc.whoAmI)
                        alreadyZapped = true;

                if (alreadyZapped) continue;

                float dist = Vector2.Distance(npc.Center, from.Center);
                if (dist < bestDist)
                {
                    best = npc;
                    bestDist = dist;
                }
            }
            return best;
        }

        private static void DrawGreenBeam(Vector2 start, Vector2 end)
        {
            int dustCount = 10;
            for (int i = 0; i < dustCount; i++)
            {
                Vector2 pos = Vector2.Lerp(start, end, i / (float)dustCount);
                Dust dust = Dust.NewDustPerfect(pos, DustID.Electric, Vector2.Zero, 0, Color.LimeGreen, 1.5f);
                dust.noGravity = true;
            }
        }
    }
}
