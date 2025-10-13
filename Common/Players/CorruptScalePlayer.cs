using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class CorruptScalePlayer : ModPlayer
    {
        public bool hasCorruptScaleSet = false;
        private int auraTick = 0;

        public override void ResetEffects()
        {
            hasCorruptScaleSet = false;
        }

        public override void PostUpdateEquips()
        {
            if (!hasCorruptScaleSet) return;

            // Apply set bonus stats
            Player.GetDamage(DamageClass.Generic) *= 0.75f;
            Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.55f;
        }

        public override void PostUpdate()
        {
            if (!hasCorruptScaleSet) return;
            if (Main.rand.NextBool(3))
            {
                float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                float distance = Main.rand.NextFloat(0f, 10 * 16); // 10 tiles
                Vector2 position = Player.Center + new Vector2(
                    (float)Math.Cos(angle) * distance,
                    (float)Math.Sin(angle) * distance
                );

                Dust dust = Dust.NewDustPerfect(position, DustID.Shadowflame, Vector2.Zero, 100, default, 1.2f);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.fadeIn = 0.8f;
            }
            auraTick++;
            if (auraTick >= 20)
            {
                auraTick = 0;
                DamageNearbyEnemies();
            }
        }

        private void DamageNearbyEnemies()
        {
            float auraRadius = 160;
            float baseDamage = 15f;
            
            // Apply pacifist damage multiplier
            float pacifistMult = Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier;
            int finalDamage = (int)(baseDamage * pacifistMult);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                    continue;

                float distance = Vector2.Distance(Player.Center, npc.Center);
                if (distance <= auraRadius)
                {
                    // Deal damage with StrikeNPC
                    NPC.HitInfo hit = new NPC.HitInfo();
                    hit.Damage = finalDamage;
                    hit.Knockback = 0f;
                    hit.HitDirection = (npc.Center.X > Player.Center.X) ? 1 : -1;
                    
                    npc.StrikeNPC(hit);
                    npc.AddBuff(ModContent.BuffType<Corrupted>(), 60);
                    
                    // Visual feedback
                    if (Main.rand.NextBool(4))
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame, 0f, 0f, 100, default, 0.8f);
                    }

                    // Sync in multiplayer
                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        NetMessage.SendStrikeNPC(npc, hit);
                    }
                }
            }
        }
    }
}