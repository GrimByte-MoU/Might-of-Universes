using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class OrcusSetPlayer : ModPlayer
    {
        private const int MaxSoulBonus = 150;
        private const float AuraTiles = 20f;
        private const float AuraRadius = AuraTiles * 16f;
        private const int Samples = 24;

        private bool wearing;
        private int tick;
        public float orcusChestplatePen;

        public override void ResetEffects()
        {
            wearing = false;
            orcusChestplatePen = 0f;
        }

        public override void UpdateEquips()
        {
            if (Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.MaskofOrcus>()
             && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.ChestplateofOrcus>()
             && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.GreavesofOrcus>())
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
                Player.setBonus = "+150 max souls\n20‑tile golden aura: enemies inside take +25% Reaper damage & gain Rebuking Light.\nHolding Orcus: +50% damage, +20% speed, ignore 25 defense.";
        }

        public override void PostUpdate()
        {
            if (!wearing) return;

            float pulse = 0.65f + 0.35f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.8f + Player.whoAmI);

            for (int i = 0; i < Samples; i++)
            {
                float ang = MathHelper.TwoPi * i / Samples;
                Vector2 dir = new((float)Math.Cos(ang), (float)Math.Sin(ang));
                Vector2 outer = Player.Center + dir * AuraRadius;
                Vector2 inner = Player.Center + dir * (AuraRadius * 0.85f);
                Lighting.AddLight(outer, 0.95f * pulse, 0.90f * pulse, 0.75f * pulse);
                Lighting.AddLight(inner, 0.45f * pulse, 0.42f * pulse, 0.35f * pulse);
            }
            Lighting.AddLight(Player.Center, 0.12f * pulse, 0.10f * pulse, 0.08f * pulse);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                float rSq = AuraRadius * AuraRadius;
                int buff = ModContent.BuffType<Content.Items.Buffs.RebukingLight>();
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (n?.active != true || n.friendly || n.townNPC || n.lifeMax <= 5) continue;
                    if (Vector2.DistanceSquared(n.Center, Player.Center) <= rSq)
                        n.AddBuff(buff, 10);
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (!wearing) return;
            Item held = Player.HeldItem;
            if (held != null && held.type == ModContent.ItemType<Content.Items.Weapons.Orcus>())
            {
                var rd = ModContent.GetInstance<ReaperDamageClass>();
                Player.GetDamage(rd) += 0.50f;
                Player.GetAttackSpeed(rd) += 0.20f;
                Player.GetArmorPenetration(rd) += 25;
            }
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) =>
            Apply(item.DamageType, item.type, target, ref modifiers);

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            Item held = proj.owner >= 0 ? Main.player[proj.owner].HeldItem : null;
            Apply(proj.DamageType, held?.type ?? 0, target, ref modifiers);
        }

        private void Apply(DamageClass dmg, int heldType, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!wearing) return;
            if (target == null || !target.active) return;
            if (dmg != ModContent.GetInstance<ReaperDamageClass>()) return;

            if (Vector2.DistanceSquared(target.Center, Player.Center) <= AuraRadius * AuraRadius)
            {
                modifiers.SourceDamage *= 1.25f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    target.AddBuff(ModContent.BuffType<Content.Items.Buffs.RebukingLight>(), 180);
            }

            if (heldType == ModContent.ItemType<Content.Items.Weapons.Orcus>())
            {
                int def = target.defense;
                if (def > 0)
                {
                    float newEff = Math.Max(0f, (def - 25f) / (float)def);
                    modifiers.DefenseEffectiveness *= newEff;
                }
            }

            if (orcusChestplatePen > 0f && target.HasBuff(ModContent.BuffType<Content.Items.Buffs.RebukingLight>()))
            {
                float frac = MathHelper.Clamp(orcusChestplatePen / 100f, 0f, 0.9f);
                modifiers.DefenseEffectiveness *= 1f - frac;
            }
        }
    }
}