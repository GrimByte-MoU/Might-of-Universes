using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class ReaperAccessoryPlayer : ModPlayer
    {
        // =========================
        // Soul Cost Modification
        // =========================
        public float SoulCostMultiplier;
        public int   SoulCostFlatReduction;
        public const float MinEffectiveCostMultiplier = 0.10f;
        public float EmpowerCostMultiplier;

        public float EmpowerDurationMultiplier;
        public int   EmpowerExtraDurationTicks;

        // =========================
        // Refund Mechanics
        // =========================
        public float RefundChance;
        public float RefundFraction;
        public int   GuaranteedSoulRefund;
        public bool  RefundAtLeastOne;

        // =========================
        // Spend Tracking
        // =========================
        public int LastSoulSpendAmount;
        public int LargestSoulSpendWindow;
        public int LargestSoulSpendTimer;
        public int LargestWindowDuration = 60 * 10;
        public bool UseLargestSpendWindowForFerryman;

        // =========================
        // Ferryman’s Token (cheat-death)
        // =========================
        public bool HasFerrymansToken;
        public int  FerrymanCooldownTicks;
        public const int FerrymanBaseCooldown = 60 * 60;
        public float FerrymanCooldownMultiplier;
        public int   FerrymanCooldownFlatReduction;
        public float FerrymanHealScale;
        public int   FerrymanMaxHeal;
        public int   FerrymanMinHealFloor = 1;
        public bool  FerrymanSetLifeToBrink = true;

        public bool  ClearOnlyDebuffs = true;

        // =========================
        // Crit / Misc Flags
        // =========================
        public bool HasUndertakersBrooch;
        public int  ReaperCritCounter;
        public int  CritsPerReward = 5;
        public int  CritRewardSouls = 5;

        public bool HasGravediggersRing;
        public bool HasCharmOfDepths;
        public bool HasMonkeysPaw;
        public bool HasSoulSiphoningArtifact;

        // =========================
        // Accessory flags (soul utility line)
        // =========================
        public bool accTatteredCharm;
        public bool accProtectionCharm;
        public bool accSpiritString;
        public bool accThreadsOfSoul;
        public bool accHematicLocket;
        public bool accIchorveinLocket;
        public bool accShackledArtifact;
        public bool accSoulEnslavementArtifact;
        public bool accSpectercageArtifact;
        public bool accSkeletonKnickknack;
        public bool accSpiritTalisman;
        public bool accVisceraNovelty;

        public int   flatMaxSoulsBonus;
        public float maxSoulsFromHPPercent;
        private float reportedPassiveSoulGenPS;
        private float accessoryPassiveSoulGenPS;
        private float passiveSoulToHpScalar;
        private int currentSoulsCached;
        private int maxSoulsCached;

        private int threadsPanicTimer;
        private int threadsPanicCooldown;
        private int visceraPulseTimer;
        private int consumePulseICD;

public bool accLoomOfFate = false;
public bool loomOfFateActive = false;
public bool loomOfFateOnCooldown = false;
public int loomOfFateDuration = 0;
public int loomOfFateCooldown = 0;

        public override void ResetEffects()
        {
            SoulCostMultiplier = 1f;
            SoulCostFlatReduction = 0;
            RefundChance = 0f;
            RefundFraction = 0f;
            GuaranteedSoulRefund = 0;
            RefundAtLeastOne = true;

            EmpowerCostMultiplier = 1f;
            EmpowerDurationMultiplier = 1f;
            EmpowerExtraDurationTicks = 0;

            HasFerrymansToken = false;
            FerrymanCooldownMultiplier = 1f;
            FerrymanCooldownFlatReduction = 0;
            FerrymanHealScale = 0.5f;
            FerrymanMaxHeal = 0;

            HasUndertakersBrooch = false;
            HasGravediggersRing = false;
            HasCharmOfDepths = false;
            HasMonkeysPaw = false;
            HasSoulSiphoningArtifact = false;

            UseLargestSpendWindowForFerryman = false;
            ClearOnlyDebuffs = true;

            accTatteredCharm = false;
            accProtectionCharm = false;
            accSpiritString = false;
            accThreadsOfSoul = false;
            accHematicLocket = false;
            accIchorveinLocket = false;
            accShackledArtifact = false;
            accSoulEnslavementArtifact = false;
            accSpectercageArtifact = false;
            accSkeletonKnickknack = false;
            accSpiritTalisman = false;
            accVisceraNovelty = false;
            accLoomOfFate = false;
            flatMaxSoulsBonus = 0;
            maxSoulsFromHPPercent = 0f;
            passiveSoulToHpScalar = 0f;
            reportedPassiveSoulGenPS = 0f;
            accessoryPassiveSoulGenPS = 0f;
        }
        public override void PostUpdateEquips()
        {
            var reaper = Player.GetModPlayer<ReaperPlayer>();
            int extraFromHP = (int)Math.Floor(Player.statLifeMax2 * maxSoulsFromHPPercent);
            int totalExtra = flatMaxSoulsBonus + extraFromHP;
            if (totalExtra != 0)
                reaper.maxSoulEnergy += totalExtra;

            UpdateSoulState(Player, (int)Math.Floor(reaper.soulEnergy), (int)Math.Floor(reaper.maxSoulEnergy));
            if (reaper.soulEnergy > reaper.maxSoulEnergy)
                reaper.soulEnergy = reaper.maxSoulEnergy;
        }

        public override void PostUpdate()
        {
            if (FerrymanCooldownTicks > 0)
                FerrymanCooldownTicks--;

            if (LargestSoulSpendTimer > 0)
            {
                LargestSoulSpendTimer--;
                if (LargestSoulSpendTimer <= 0)
                {
                    LargestSoulSpendTimer = 0;
                    LargestSoulSpendWindow = 0;
                }
            }

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            if (reaper.justConsumedSouls)
            {
                OnSoulsConsumed(Player, 1);
            }

            if (accThreadsOfSoul)
            {
                if (threadsPanicCooldown > 0) threadsPanicCooldown--;
                if (threadsPanicTimer > 0)
                {
                    threadsPanicTimer--;
                    accessoryPassiveSoulGenPS += 20f;
                }
                else if (Player.statLife <= Player.statLifeMax2 * 0.20f && threadsPanicCooldown <= 0)
                {
                    threadsPanicTimer = 60 * 5;
                    threadsPanicCooldown = 60 * 20;
                }
            }

            if (accSkeletonKnickknack && IsBelowSoulPercent(0.30f))
            {
                accessoryPassiveSoulGenPS += 3f;
            }

            if (accVisceraNovelty)
            {
                if (IsBelowSoulPercent(0.50f))
                {
                    accessoryPassiveSoulGenPS += 5f;
                }

                if (--visceraPulseTimer <= 0)
                {
                    visceraPulseTimer = 60 * 3;
                    DoPulseAOE(Player, radiusTiles: 30, damage: 125, applyIchor: true, applySpineless: true, applyEnemyBleeding: true, durationSeconds: 3);
                }
            }

            float totalPS = reportedPassiveSoulGenPS + accessoryPassiveSoulGenPS;
            if (totalPS > 0f && passiveSoulToHpScalar > 0f)
            {
                float hpPerSecond = totalPS * passiveSoulToHpScalar;
                Player.lifeRegen += (int)Math.Round(hpPerSecond * 2f);
            }
        }

        public void RecordSoulSpend(int finalSpent)
        {
            LastSoulSpendAmount = finalSpent;
            if (finalSpent > LargestSoulSpendWindow)
            {
                LargestSoulSpendWindow = finalSpent;
                LargestSoulSpendTimer  = LargestWindowDuration;
            }
        }

        public void RegisterReaperCrit()
        {
            if (!HasUndertakersBrooch)
                return;

            ReaperCritCounter++;
            if (ReaperCritCounter >= CritsPerReward)
            {
                ReaperCritCounter = 0;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.AddSoulEnergy(CritRewardSouls, Player.Center);
            }
        }

        private int ComputeFerrymanCooldown()
        {
            int baseTicks = (int)(FerrymanBaseCooldown * FerrymanCooldownMultiplier) - FerrymanCooldownFlatReduction;
            return Math.Max(60, baseTicks);
        }

        // =========================
        // NEW CORE: Consume all souls
        // =========================
        private int ComputeHealFromConsumed(int soulsConsumed)
        {
            double raw = soulsConsumed * FerrymanHealScale;
            int heal = (int)Math.Ceiling(raw);
            if (heal < FerrymanMinHealFloor) heal = FerrymanMinHealFloor;
            if (FerrymanMaxHeal > 0) heal = Math.Min(heal, FerrymanMaxHeal);
            return heal;
        }

        public bool TryActivateFerrymanFatalSave(int incomingDamage)
        {
            if (!HasFerrymansToken || FerrymanCooldownTicks > 0)
                return false;

            if (incomingDamage < Player.statLife)
                return false;

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            int soulsCurrent = (int)Math.Floor(reaper.soulEnergy);

            if (soulsCurrent <= 0)
                return false;

            reaper.soulEnergy = 0f;

            int heal = ComputeHealFromConsumed(soulsCurrent);

            if (FerrymanSetLifeToBrink)
                Player.statLife = Math.Min(Player.statLifeMax2, 1 + heal);
            else
                Player.statLife = Math.Min(Player.statLifeMax2, Player.statLife + heal);

            Player.HealEffect(heal, broadcast: true);

            for (int i = Player.buffType.Length - 1; i >= 0; i--)
            {
                int b = Player.buffType[i];
                if (b <= 0) continue;

                if (ClearOnlyDebuffs)
                {
                    if (Main.debuff[b])
                        Player.DelBuff(i);
                }
                else
                {
                    Player.DelBuff(i);
                }
            }

            Player.immune = true;
            Player.immuneTime = Math.Max(Player.immuneTime, 60);

            for (int i = 0; i < 40; i++)
            {
                var d = Dust.NewDustDirect(Player.position, Player.width, Player.height, 15,
                    Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, -0.5f));
                d.scale = 1.15f + Main.rand.NextFloat(0.25f);
                d.noGravity = true;
            }

            FerrymanCooldownTicks = ComputeFerrymanCooldown();

            CombatText.NewText(Player.getRect(), Color.Gold, $"Souls: {soulsCurrent}");

            return true;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (TryActivateFerrymanFatalSave(info.Damage))
                return true;
            return false;
        }

        public override void OnRespawn()
        {
        }

        public float GetFerrymanCooldownFraction()
        {
            int denom = ComputeFerrymanCooldown();
            if (FerrymanCooldownTicks <= 0) return 0f;
            return Math.Clamp(FerrymanCooldownTicks / (float)denom, 0f, 1f);
        }

        public int GetFerrymanCooldownSeconds()
        {
            if (FerrymanCooldownTicks <= 0) return 0;
            return (int)Math.Ceiling(FerrymanCooldownTicks / 60f);
        }

        public string GetFerrymanStatusText()
        {
            if (!HasFerrymansToken)
                return "Ferryman inactive";
            return FerrymanCooldownTicks > 0
                ? $"Ferryman cooldown: {GetFerrymanCooldownSeconds()}s"
                : "Ferryman ready";
        }

        // =========================
        // Accessory integration helpers
        // =========================

        public static void OnSoulsConsumed(Player player, int amount)
        {
            var p = player.GetModPlayer<ReaperAccessoryPlayer>();

            if (p.consumePulseICD > 0) { p.consumePulseICD--; return; }
            p.consumePulseICD = 6;

            if (p.accTatteredCharm)
                player.Heal(5);

            if (p.accProtectionCharm)
                player.AddBuff(ModContent.BuffType<ProtectionCantrip>(), 60 * 5);

            if (p.accSpiritTalisman)
                player.AddBuff(ModContent.BuffType<SpiritsTag>(), 60 * 5);

            if (p.accHematicLocket)
                p.DoPulseAOE(player, radiusTiles: 15, damage: 50, applyIchor: false, applySpineless: true, applyEnemyBleeding: false, durationSeconds: 3);

            if (p.accIchorveinLocket)
                p.DoPulseAOE(player, radiusTiles: 25, damage: 75, applyIchor: true, applySpineless: true, applyEnemyBleeding: false, durationSeconds: 3);

            if (p.accVisceraNovelty)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 vel = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, -1f));
                    Projectile.NewProjectile(player.GetSource_Accessory(player.HeldItem), player.Center, vel,
                        ModContent.ProjectileType<VisceraBoltProj>(), 150, 2f, player.whoAmI);
                }
            }
        }

        public static void ReportPassiveSoulGain(Player player, float soulsPerSecond)
        {
            if (soulsPerSecond > 0f)
                player.GetModPlayer<ReaperAccessoryPlayer>().reportedPassiveSoulGenPS += soulsPerSecond;
        }

        public static void UpdateSoulState(Player player, int currentSouls, int maxSouls)
        {
            var p = player.GetModPlayer<ReaperAccessoryPlayer>();
            p.currentSoulsCached = currentSouls;
            p.maxSoulsCached = Math.Max(1, maxSouls);
        }

        public int GetExtraMaxSouls(int baseMaxSouls)
        {
            int extraFromHP = (int)Math.Floor(Player.statLifeMax2 * maxSoulsFromHPPercent);
            return flatMaxSoulsBonus + extraFromHP;
        }

        public bool IsBelowSoulPercent(float fraction)
        {
            if (maxSoulsCached <= 0) return false;
            return currentSoulsCached <= maxSoulsCached * fraction;
        }

        public void ApplyMaxSoulFromHP(float percent)
        {
            maxSoulsFromHPPercent = Math.Max(maxSoulsFromHPPercent, percent);
        }

        public void ApplyPassiveSoulToHPScalar(float scalar)
        {
            passiveSoulToHpScalar = Math.Max(passiveSoulToHpScalar, scalar);
        }

        public void AddAccessoryPassiveSoulGenPS(float soulsPerSecond)
        {
            accessoryPassiveSoulGenPS += soulsPerSecond;
        }
        
        private void DoPulseAOE(Player player, int radiusTiles, int damage, bool applyIchor, bool applySpineless, bool applyEnemyBleeding, int durationSeconds)
        {
            float radius = radiusTiles * 16f;
            int durationTicks = durationSeconds * 60;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.life <= 0 || npc.dontTakeDamage) continue;
                if (Vector2.DistanceSquared(npc.Center, player.Center) > radius * radius) continue;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.HitInfo hit = new NPC.HitInfo
                    {
                        Damage = damage,
                        Knockback = 2f,
                        HitDirection = player.direction,
                        Crit = false
                    };
                    npc.StrikeNPC(hit);
                }

                if (applyIchor) npc.AddBuff(BuffID.Ichor, durationTicks);
                if (applySpineless) npc.AddBuff(ModContent.BuffType<Spineless>(), durationTicks);
                if (applyEnemyBleeding) npc.AddBuff(ModContent.BuffType<EnemyBleeding>(), durationTicks);
            }
        }
    }
}