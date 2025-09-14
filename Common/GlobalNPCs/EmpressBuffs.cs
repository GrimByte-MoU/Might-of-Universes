using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.NPCs
{
    // Agreed changes:
    // - Empress of Light gets +5 defense.
    // - All damage from Empress (contact and her projectiles) is reduced by 10% (to smooth spikes).
    // - Phase 1 (HP > 50%):
    //     * Contact: applies Rebuking Light for 3.0s.
    //     * Projectiles: apply Rebuking Light for 1.0s.
    // - Phase 2 (HP <= 50%):
    //     * Same Rebuking Light applications as Phase 1.
    //     * Additionally applies Prismatic Rend: 2.0s on contact, 0.5s on projectile hits.
    // - Debuff durations scale by difficulty: Expert +50%, Master +100%.

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

            // Reduce contact damage by 10%
            modifiers.FinalDamage *= 0.9f;

            bool phase2 = IsEmpressPhase2(npc);
            float mult = GetDifficultyDurationMultiplier();

            // Phase 1 and 2: Rebuking Light on contact for 3.0s base
            int rebukeContact = (int)(180 * mult);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                target.AddBuff(ModContent.BuffType<RebukingLight>(), rebukeContact);
            }

            if (phase2)
            {
                // Additional Prismatic Rend on contact in Phase 2: 2.0s base
                int rendContact = (int)(120 * mult);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    target.AddBuff(ModContent.BuffType<PrismaticRend>(), rendContact);
                }
            }
        }

        internal static float GetDifficultyDurationMultiplier()
        {
            if (Main.masterMode) return 2f;     // +100%
            if (Main.expertMode) return 1.5f;   // +50%
            return 1f;                           // Normal
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
        // Allowlist of vanilla Empress of Light projectile IDs (1.4.4+):
        // These are the internal projectile IDs referenced by the boss’ attacks.
        // Known IDs from the official wiki listing: 872, 873, 874, 919, 923.
        // They correspond to her Prismatic Bolt / Sun Dance / Everlasting Rainbow / Ethereal Lance patterns.
        private static readonly HashSet<int> EmpressProjectileTypes = new()
        {
            872, // Empress projectile (e.g., Prismatic Bolt variant)
            873, // Empress projectile
            874, // Empress projectile
            919, // Empress projectile (e.g., Everlasting Rainbow segment)
            923, // Empress projectile (e.g., Ethereal Lance)
        };

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            // Only care about hostile projectiles
            if (!projectile.hostile)
                return;

            // Must be one of Empress's projectiles
            if (!EmpressProjectileTypes.Contains(projectile.type))
                return;

            // Find nearest active Empress to this player and ensure we're within influence radius (MP safety)
            if (!EmpressBuffs.TryGetNearestEmpress(target, out NPC empress, out float dist) || dist > EmpressBuffs.InfluenceRadius)
                return;

            // Reduce projectile damage by 10%
            modifiers.FinalDamage *= 0.9f;

            bool phase2 = EmpressBuffs.IsEmpressPhase2(empress);
            float mult = EmpressBuffs.GetDifficultyDurationMultiplier();

            // Phase 1 and 2: Rebuking Light on projectile hits for 1.0s base
            int rebukeProj = (int)(60 * mult);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                target.AddBuff(ModContent.BuffType<RebukingLight>(), rebukeProj);
            }

            if (phase2)
            {
                // Additional Prismatic Rend on projectile hits in Phase 2: 0.5s base
                int rendProj = (int)(30 * mult);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    target.AddBuff(ModContent.BuffType<PrismaticRend>(), rendProj);
                }
            }
        }
    }
}

