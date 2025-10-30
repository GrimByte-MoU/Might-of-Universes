using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Armors;
using MightofUniverses.Common;
using Terraria.Audio;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class MechaCactusSetPlayer : ModPlayer
    {
        private const float MaxSoulBonus = 150f;
        private const float DamageThresholdFraction = 0.10f;
        private const float ThornsDamageMultiplier = 2.5f;
        private const int   MortalWoundDuration = 300;
        private const int   SoulGainCooldownTicks = 180;
        private const int   ThornDustCount = 20;

        private bool wearingSet;
        private int soulGainCooldown;

        public override void ResetEffects()
        {
            wearingSet = false;
        }

        public override void UpdateEquips()
        {
            if (IsWearingFullMechaCactusSet())
            {
                wearingSet = true;

                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;

                reaper.maxSoulEnergy += MaxSoulBonus;
            }

            if (soulGainCooldown > 0)
                soulGainCooldown--;
        }

        public override void PostUpdateEquips()
        {
            if (wearingSet)
            {
                Player.setBonus =
                    $"+{(int)MaxSoulBonus} max souls\n" +
                    "Taking >10% of max life in one hit retaliates for 250% of damage, applies Mortal Wound for 5 seconds, and grants souls equal to damage taken.\n" +
                    "Enemies with Mortal Wound deal 20% less damage to you and the soul gain has a 3 second cooldown.";
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (!wearingSet) return;
            if (npc != null && npc.HasBuff(ModContent.BuffType<MortalWound>()))
            {
                modifiers.FinalDamage *= 0.80f;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (!wearingSet) return;
            if (proj != null && proj.hostile && proj.owner >= 0 && proj.owner < Main.maxNPCs)
            {
                NPC ownerNPC = Main.npc[proj.owner];
                if (ownerNPC != null && ownerNPC.active && ownerNPC.HasBuff(ModContent.BuffType<MortalWound>()))
                {
                    modifiers.FinalDamage *= 0.80f;
                }
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (!wearingSet || !Player.active || Player.dead) return;
            TryRetaliateAndSoulGain(npc?.Center ?? Player.Center, npc, hurtInfo.Damage);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (!wearingSet || !Player.active || Player.dead) return;
            NPC sourceNPC = null;
            if (proj != null && proj.owner >= 0 && proj.owner < Main.maxNPCs)
                sourceNPC = Main.npc[proj.owner];

            TryRetaliateAndSoulGain(proj?.Center ?? Player.Center, sourceNPC, hurtInfo.Damage);
        }

        private void TryRetaliateAndSoulGain(Vector2 impactCenter, NPC sourceNpc, int damageTaken)
        {
            if (damageTaken <= 0) return;

            float threshold = Player.statLifeMax2 * DamageThresholdFraction;
            if (damageTaken <= threshold) return;

            int thornsDamage = (int)Math.Max(1, damageTaken * ThornsDamageMultiplier);

            if (sourceNpc != null && sourceNpc.active && !sourceNpc.friendly && !sourceNpc.townNPC)
            {
                sourceNpc.StrikeNPC(new NPC.HitInfo
                {
                    Damage = thornsDamage,
                    Knockback = 0f,
                    HitDirection = Player.direction
                });

                sourceNpc.AddBuff(ModContent.BuffType<MortalWound>(), MortalWoundDuration);
            }

            PlayThornEffects(impactCenter, thornsDamage);

            if (soulGainCooldown <= 0)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.AddSoulEnergy(damageTaken, impactCenter);
                soulGainCooldown = SoulGainCooldownTicks;
            }
        }

        private void PlayThornEffects(Vector2 at, int thornsDamage)
        {
            SoundEngine.PlaySound(SoundID.Item70.WithPitchOffset(-0.3f), at);

            for (int i = 0; i < ThornDustCount; i++)
            {
                var vel = Main.rand.NextVector2Circular(4f, 4f);
                int d = Dust.NewDust(at - new Vector2(4, 4), 8, 8, DustID.GreenFairy, vel.X, vel.Y, 150, default, 1.1f);
                Main.dust[d].noGravity = true;
            }

            CombatText.NewText(
                new Rectangle((int)at.X - 4, (int)at.Y - 4, 8, 8),
                Microsoft.Xna.Framework.Color.LimeGreen,
                thornsDamage,
                dramatic: true
            );
        }

        private bool IsWearingFullMechaCactusSet()
        {
            return Player.armor[0].type == ModContent.ItemType<MechaCactusHelmet>()
                && Player.armor[1].type == ModContent.ItemType<MechaCactusBreastplate>()
                && Player.armor[2].type == ModContent.ItemType<MechaCactusLeggings>();
        }

        public override void PostUpdate()
        {
            if (!wearingSet) return;

            Lighting.AddLight(Player.Center, 0f, 0.8f, 0.2f);

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.GreenFairy, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(3))
            {
                int dustType = Main.rand.NextBool() ? DustID.GreenFairy : DustID.Titanium;
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, dustType, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }
    }
}