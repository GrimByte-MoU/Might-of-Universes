using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.DataStructures;
using System;

namespace MightofUniverses.Common.Players
{
    public class CyberSpherePlayer : ModPlayer
    {
        public bool hasCyberSphere;
        private const float RADIUS = 20f * 16f;
        private const int BASE_DAMAGE = 40;
        private const int DAMAGE_PER_ENEMY = 5;
        private int damageTimer;

        public override void ResetEffects()
        {
            hasCyberSphere = false;
        }

        public override void PostUpdate()
        {
            if (!hasCyberSphere) return;

            const int NUM_POINTS = 72;
            for (int i = 0; i < NUM_POINTS; i++)
            {
                float angle = (float)(i * (2 * Math.PI) / NUM_POINTS);
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle) * RADIUS,
                    (float)Math.Sin(angle) * RADIUS
                );
                Vector2 dustPos = Player.Center + offset;
                
                Dust dust = Dust.NewDustPerfect(
                    dustPos,
                    DustID.GreenTorch,
                    Vector2.Zero,
                    0,
                    Color.White,
                    1f
                );
                dust.noGravity = true;
                dust.noLight = true;
            }

            damageTimer++;
            if (damageTimer >= 60)
            {
                damageTimer = 0;
                List<NPC> affectedNPCs = new List<NPC>();

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Player.Center) <= RADIUS)
                    {
                        affectedNPCs.Add(npc);
                    }
                }

                if (affectedNPCs.Count > 0)
                {
                    int damagePerNPC = BASE_DAMAGE + (affectedNPCs.Count * DAMAGE_PER_ENEMY);
                    foreach (NPC npc in affectedNPCs)
                    {
                       npc.StrikeNPC(new NPC.HitInfo
{
    Damage = damagePerNPC,
    Knockback = 0f,
    HitDirection = 0
});

                        
                        for (int d = 0; d < 3; d++)
                        {
                            Dust.NewDust(
                                npc.position,
                                npc.width,
                                npc.height,
                                DustID.GreenTorch,
                                0f,
                                0f,
                                100,
                                default,
                                1f
                            );
                        }
                    }
                }
            }
        }
    }
}
