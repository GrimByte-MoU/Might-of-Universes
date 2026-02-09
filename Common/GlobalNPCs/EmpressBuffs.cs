using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalNPCs
{

    public class EmpressBuffs : GlobalNPC
    {
        internal const float InfluenceRadius = 4000f;

        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.HallowBoss)
            {
                npc.defense += 5;
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (npc.type != NPCID.HallowBoss)
                return;

            modifiers.FinalDamage *= 0.9f;

            bool phase2 = IsEmpressPhase2(npc);
            float mult = GetDifficultyDurationMultiplier();

            int rebukeContact = (int)(180 * mult);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                target.AddBuff(ModContent.BuffType<RebukingLight>(), rebukeContact);
            }

            if (phase2)
            {
                int rendContact = (int)(120 * mult);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    target.AddBuff(ModContent.BuffType<PrismaticRend>(), rendContact);
                }
            }
        }

        internal static float GetDifficultyDurationMultiplier()
        {
            if (Main.masterMode) return 2f;
            if (Main.expertMode) return 1.5f;   
            return 1f;                           
        }

        internal static bool TryGetNearestEmpress(Player player, out NPC empress, out float distance)
        {
            empress = null;
            distance = float.MaxValue;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (!n.active || n.type != NPCID.HallowBoss)
                    continue;

                float d = Vector2.Distance(player.Center, n.Center);
                if (d < distance)
                {
                    distance = d;
                    empress = n;
                }
            }
            return empress != null;
        }

        internal static bool IsEmpressPhase2(NPC empress)
        {
            if (empress == null || !empress.active || empress.type != NPCID.HallowBoss)
                return false;

            return empress.life <= empress.lifeMax / 2;
        }
    }

    public class EmpressProjectileDebuffs : GlobalProjectile
    {
        private static readonly HashSet<int> EmpressProjectileTypes = new()
        {
            872,
            873,
            874,
            919,
            923,
        };

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {

            if (!projectile.hostile)
                return;

            if (!EmpressProjectileTypes.Contains(projectile.type))
                return;

            if (!EmpressBuffs.TryGetNearestEmpress(target, out NPC empress, out float dist) || dist > EmpressBuffs.InfluenceRadius)
                return;

            modifiers.FinalDamage *= 0.9f;

            bool phase2 = EmpressBuffs.IsEmpressPhase2(empress);
            float mult = EmpressBuffs.GetDifficultyDurationMultiplier();

            int rebukeProj = (int)(60 * mult);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                target.AddBuff(ModContent.BuffType<RebukingLight>(), rebukeProj);
            }

            if (phase2)
            {
                int rendProj = (int)(30 * mult);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    target.AddBuff(ModContent.BuffType<PrismaticRend>(), rendProj);
                }
            }
        }
    }
}

