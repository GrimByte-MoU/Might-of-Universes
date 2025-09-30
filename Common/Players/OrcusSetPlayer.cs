using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class OrcusSetPlayer : ModPlayer
    {
        private const int SetBonusMaxSouls = 150;
        private const float AuraRadiusTiles = 20f;
        private const float AuraRadius = AuraRadiusTiles * 16f;
        private const float CircleDustInterval = 12f;
        private bool wearingOrcusSet;
        private int dustTick;

        public float orcusChestplatePen;

        public override void ResetEffects()
        {
            wearingOrcusSet = false;
            orcusChestplatePen = 0f;
        }

        public override void UpdateEquips()
        {
            if (IsWearingFullOrcusSet())
            {
                wearingOrcusSet = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;

                var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
                acc.flatMaxSoulsBonus += SetBonusMaxSouls;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingOrcusSet)
            {
                Player.setBonus = $"+{SetBonusMaxSouls} max souls\nCreates a 20-tile golden-white aura around you. Enemies inside take +25% Reaper damage and are afflicted with Rebuking Light.\n While wielding Orcus, it will deal +50% damage, swing 20% faster and ignore 25 defense.";
            }
        }

        public override void PostUpdate()
        {
            if (!wearingOrcusSet)
                return;

            // Halo: pulsing sampled lights around the circle + sparse sparkles
            dustTick++;
            // Pulse speed and intensity
            float pulse = 0.65f + 0.35f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.8f + Player.whoAmI);
            int lightSamples = 24; // number of light samples around the circle (24 is a good balance)
            float innerMultiplier = 0.55f * pulse; // inner glow
            float outerMultiplier = 1.0f * pulse;  // ring brightness

            for (int i = 0; i < lightSamples; i++)
            {
                float ang = MathHelper.TwoPi * i / lightSamples;
                Vector2 ringPos = Player.Center + new Vector2((float)Math.Cos(ang), (float)Math.Sin(ang)) * AuraRadius;

                // Outer ring light (golden-white)
                Lighting.AddLight(ringPos, 0.95f * outerMultiplier, 0.90f * outerMultiplier, 0.75f * outerMultiplier);

                // Slight inner fill so the halo appears soft towards the center
                Vector2 innerPos = Player.Center + new Vector2((float)Math.Cos(ang), (float)Math.Sin(ang)) * (AuraRadius * 0.85f);
                Lighting.AddLight(innerPos, 0.45f * innerMultiplier, 0.42f * innerMultiplier, 0.35f * innerMultiplier);

                // Occasional tiny sparkle particles (very rare)
                if (Main.rand.NextBool(12))
                {
                    Vector2 jitter = Main.rand.NextVector2Circular(6f, 6f);
                    var d = Dust.NewDustDirect(ringPos + jitter - new Vector2(2f, 2f), 4, 4, DustID.GoldCoin, 0f, 0f, 150, Color.White, 1.1f);
                    d.noGravity = true;
                    d.velocity = (d.position - Player.Center).SafeNormalize(Vector2.Zero) * 0.25f;
                    d.fadeIn = 0.6f;
                    d.fadeIn = 0.6f;
                }
            }

            // A softer central light pulse (for halo bloom)
            Lighting.AddLight(Player.Center, 0.12f * pulse, 0.10f * pulse, 0.08f * pulse);

            // Keep Rebuking Light applied to NPCs inside the circle (server-side)
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc != null && npc.active && !npc.friendly && !npc.townNPC && npc.lifeMax > 5)
                    {
                        if (Vector2.Distance(npc.Center, Player.Center) <= AuraRadius)
                        {
                            npc.AddBuff(ModContent.BuffType<Content.Items.Buffs.RebukingLight>(), 10);
                        }
                    }
                }
            }
        }

        private bool IsWearingFullOrcusSet()
        {
            return Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.MaskofOrcus>()
                && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.ChestplateofOrcus>()
                && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.GreavesofOrcus>();
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            ApplyOrcusSetEffects(item.DamageType, item.type, target, ref modifiers);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            int owner = proj.owner;
            Item held = owner >= 0 ? Main.player[owner].HeldItem : null;
            int heldType = held != null ? held.type : 0;
            ApplyOrcusSetEffects(proj.DamageType, heldType, target, ref modifiers);
        }

        private void ApplyOrcusSetEffects(DamageClass damageType, int heldItemType, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target == null || !target.active) return;

            if (damageType != ModContent.GetInstance<ReaperDamageClass>())
                return;

            if (Vector2.Distance(target.Center, Player.Center) <= AuraRadius)
            {
                modifiers.SourceDamage *= 1.25f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    target.AddBuff(ModContent.BuffType<Content.Items.Buffs.RebukingLight>(), 180);
                }
            }

            if (heldItemType == ModContent.ItemType<Content.Items.Weapons.Orcus>())
            {
                modifiers.SourceDamage *= 1.5f;

                int def = target.defense;
                if (def > 0)
                {
                    float newEffectiveness = Math.Max(0f, (def - 25f) / (float)def);
                    modifiers.DefenseEffectiveness *= newEffectiveness;
                }
            }

            if (orcusChestplatePen > 0f && target.HasBuff(ModContent.BuffType<Content.Items.Buffs.RebukingLight>()))
            {
                float penFrac = MathHelper.Clamp(orcusChestplatePen / 100f, 0f, 0.9f);
                modifiers.DefenseEffectiveness *= 1f - penFrac;
            }
        }

        public override void PostUpdateMiscEffects()
        {
            Item held = Player.HeldItem;
            if (wearingOrcusSet && held != null && held.type == ModContent.ItemType<Content.Items.Weapons.Orcus>())
            {
                var reaper = ModContent.GetInstance<ReaperDamageClass>();
                Player.GetDamage(reaper) += 0.50f;
                Player.GetArmorPenetration(reaper) += 25;
                Player.GetAttackSpeed(reaper) += 0.20f;
            }
        }
    }
}