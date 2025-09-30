using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Armors;

namespace MightofUniverses.Common.Players
{
    public class GravetenderSetPlayer : ModPlayer
    {


        private bool wearingGravetenderSet;
        private int thornCooldown;
        public float poisonArmorPen;

        public override void ResetEffects()
        {
            wearingGravetenderSet = false;
            poisonArmorPen = 0f;
        }

        public override void UpdateEquips()
        {
            if (IsWearingFullGravetenderSet())
            {
                wearingGravetenderSet = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;

                var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
                acc.flatMaxSoulsBonus += 40;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingGravetenderSet)
            {
                Player.setBonus = "+40 max souls\nWhen you take >20% of your max life in one hit, fire 6 Gravetender Thorns that pierce 1 enemy and inflict Poisoned for 2 seconds.\n Each thorn gathers 1 soul on hit.\n This ability has a 5 second cooldown.";
            }
        }

        public override void PostUpdate()
        {
            if (thornCooldown > 0) thornCooldown--;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (!wearingGravetenderSet || !Player.active || Player.dead)
                return;

            float threshold = Player.statLifeMax2 * 0.20f;
            if (hurtInfo.Damage > threshold)
            {
                TrySpawnThorns(npc?.Center ?? Player.Center);
            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (!wearingGravetenderSet || !Player.active || Player.dead)
                return;

            float threshold = Player.statLifeMax2 * 0.20f;
            if (hurtInfo.Damage > threshold)
            {
                TrySpawnThorns(proj?.Center ?? Player.Center);
            }
        }

        private void TrySpawnThorns(Vector2 origin)
        {
            if (thornCooldown > 0) return;
            thornCooldown = 300;

            // Spawn radial burst of Thorns
            for (int i = 0; i < 6; i++)
            {
                float angle = MathHelper.TwoPi * i / 6;
                Vector2 vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 8f;
                Vector2 spawnPos = Player.Center + vel.SafeNormalize(Vector2.Zero) * 10f;
                Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPos, vel, ModContent.ProjectileType<GravetenderThorn>(), 15, 0f, Player.whoAmI);
            }

            // Visuals & sound
            for (int i = 0; i < 10; i++)
            {
                var d = Dust.NewDustPerfect(Player.Center + Main.rand.NextVector2Circular(12f, 12f), DustID.Smoke, Main.rand.NextVector2Circular(1.2f, 1.2f), Alpha: 150, Color.White, Scale: 1f);
                d.noGravity = true;
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Player.position);
        }

        private bool IsWearingFullGravetenderSet()
        {
            return Player.armor[0].type == ModContent.ItemType<GravetenderHat>()
                && Player.armor[1].type == ModContent.ItemType<GravetenderChestplate>()
                && Player.armor[2].type == ModContent.ItemType<GravetenderShoes>();
        }

        // Apply armor-penetration against Poisoned enemies for both item and projectile hits
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            ApplyPoisonArmorPen(target, ref modifiers);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            ApplyPoisonArmorPen(target, ref modifiers);
        }

        private void ApplyPoisonArmorPen(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (poisonArmorPen <= 0f) return;
            if (target == null || !target.active) return;
            if (!target.HasBuff(BuffID.Poisoned)) return;

            float penFrac = MathHelper.Clamp(poisonArmorPen / 100f, 0f, 0.9f);
            modifiers.DefenseEffectiveness *= 1f - penFrac;
        }
    }
}