using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Common.Players
{
    public class FoundryRevenantPlayer : ModPlayer
    {
        public const int MaxStacks = 50;

        public bool FullSetEquipped;
        public int Stacks;

        public override void Initialize()
        {
            Stacks = 0;
        }

        public override void ResetEffects()
        {
            FullSetEquipped = false;
        }

        public override void UpdateDead()
        {
            Stacks = 0;
        }

        public override void PostUpdateEquips()
        {
            if (!FullSetEquipped)
            {
                if (Stacks > 0) Stacks = 0;
                return;
            }

            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();

            Player.GetDamage(reaperClass) += 0.10f;
            int s = Math.Clamp(Stacks, 0, MaxStacks);
            Player.GetArmorPenetration(DamageClass.Generic) += s;
            if (s >= 5)
                Player.GetDamage(reaperClass) += 0.01f * s;
            if (s >= 10)
                Player.moveSpeed += 0.01f * s;
            if (s >= 50)
                Player.endurance += 0.002f * s;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (!FullSetEquipped)
                return false;

            int s = Stacks;
            if (s >= 25)
            {
                float chance = s * 0.001f;
                if (Main.rand.NextFloat() < chance)
                {
                    SoundEngine.PlaySound(SoundID.Item29 with { Volume = 0.8f, Pitch = 0.1f }, Player.Center);
                    for (int i = 0; i < 12; i++)
                    {
                        int d = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Smoke, 0f, 0f, 120, default, 1.1f);
                        Main.dust[d].noGravity = true;
                    }
                    Player.SetImmuneTimeForAllTypes(20);
                    return true;
                }
            }

            return false;
        }

        public void OnSoulsConsumed(int amount)
        {
            if (!FullSetEquipped || amount <= 0) return;
            if (Stacks < MaxStacks)
            {
                Stacks++;
                int d = Dust.NewDust(Player.Center, 0, 0, DustID.Torch, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 160, new Color(255, 160, 80), 1.1f);
                Main.dust[d].noGravity = true;
            }
        }

        public override void PostUpdate()
        {
            if (!FullSetEquipped) return;

            Lighting.AddLight(Player.Center, 1f, 0.4f, 0f);

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }
    }
}