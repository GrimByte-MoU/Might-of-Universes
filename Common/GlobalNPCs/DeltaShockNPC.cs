using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class DeltaShockNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int pulseTimer = 0;

        public override void ResetEffects(NPC npc)
        {
            if (!npc.HasBuff<DeltaShock>())
            {
                pulseTimer = 0;
            }
        }

        public override void PostAI(NPC npc)
        {
            if (npc.HasBuff<DeltaShock>())
            {
                pulseTimer++;

                if (pulseTimer >= 120)
                {
                    pulseTimer = 0;
                    ElectricPulse(npc.Center, 15 * 16, 500, false);
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.HasBuff<DeltaShock>())
            {
                ElectricPulse(npc.Center, 15 * 16, 2000, true);
            }
        }

        private void ElectricPulse(Vector2 center, int radius, int damage, bool isDeathPulse)
        {
            SoundEngine.PlaySound(SoundID.Item122, center);

            int dustCount = isDeathPulse ? 30 : 15;
            for (int i = 0; i < dustCount; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                Dust dust = Dust.NewDustPerfect(center, DustID.Electric, velocity, 100, isDeathPulse ? Color.Yellow : Color.Cyan, isDeathPulse ? 3.0f : 2.0f);
                dust.noGravity = true;
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (target.active && !target.friendly)
                {
                    float distance = Vector2.Distance(target.Center, center);
                    if (distance <= radius)
                    {
                        target.SimpleStrikeNPC(damage, 0, false, 0f, null, false, 0f, true);

                        for (int j = 0; j < 10; j++)
                        {
                            Dust shockDust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Electric, 0f, 0f, 100, Color.Yellow, 2.0f);
                            shockDust.noGravity = true;
                            shockDust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                        }
                    }
                }
            }
        }
    }
}