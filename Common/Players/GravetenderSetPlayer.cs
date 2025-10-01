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
        private const int MaxSoulBonus = 40;

        private bool wearing;
        private int thornCooldown;
        public float poisonArmorPen;

        public override void ResetEffects()
        {
            wearing = false;
            poisonArmorPen = 0f;
        }

        public override void UpdateEquips()
        {
            if (Player.armor[0].type == ModContent.ItemType<GravetenderHat>()
             && Player.armor[1].type == ModContent.ItemType<GravetenderChestplate>()
             && Player.armor[2].type == ModContent.ItemType<GravetenderShoes>())
            {
                wearing = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;
                reaper.maxSoulEnergy += MaxSoulBonus;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearing)
                Player.setBonus = "+40 max souls\nTaking >20% max life in one hit fires 6 thorns. Each thorn gathers 1 soul and have a 5 second cooldown.";
        }

        public override void PostUpdate()
        {
            if (thornCooldown > 0) thornCooldown--;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo info)
        {
            if (!wearing || !Player.active || Player.dead) return;
            float threshold = Player.statLifeMax2 * 0.20f;
            if (info.Damage > threshold) TrySpawnThorns();
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo info)
        {
            if (!wearing || !Player.active || Player.dead) return;
            float threshold = Player.statLifeMax2 * 0.20f;
            if (info.Damage > threshold) TrySpawnThorns();
        }

        private void TrySpawnThorns()
        {
            if (thornCooldown > 0) return;
            thornCooldown = 300;

            for (int i = 0; i < 6; i++)
            {
                float angle = MathHelper.TwoPi * i / 6f;
                Vector2 vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 8f;
                Vector2 spawnPos = Player.Center + vel.SafeNormalize(Vector2.Zero) * 10f;
                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    spawnPos,
                    vel,
                    ModContent.ProjectileType<GravetenderThorn>(),
                    15,
                    0f,
                    Player.whoAmI
                );
            }

            for (int i = 0; i < 10; i++)
            {
                var d = Dust.NewDustPerfect(
                    Player.Center + Main.rand.NextVector2Circular(12f, 12f),
                    DustID.Smoke,
                    Main.rand.NextVector2Circular(1.2f, 1.2f),
                    150,
                    Color.White,
                    1f
                );
                d.noGravity = true;
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Player.position);
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) =>
            ApplyPoison(target, ref modifiers);

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) =>
            ApplyPoison(target, ref modifiers);

        private void ApplyPoison(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (poisonArmorPen <= 0f || target == null || !target.active) return;
            if (!target.HasBuff(BuffID.Poisoned)) return;
            float frac = MathHelper.Clamp(poisonArmorPen / 100f, 0f, 0.9f);
            modifiers.DefenseEffectiveness *= 1f - frac;
        }
    }
}