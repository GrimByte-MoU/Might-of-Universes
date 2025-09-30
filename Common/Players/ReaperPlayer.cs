using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;

namespace MightofUniverses.Common.Players
{
    public class ReaperPlayer : ModPlayer
    {
        public float soulEnergy;
        public float maxSoulEnergy = 100f;
        public float soulGatherMultiplier = 1f;
        public bool hasReaperArmor;
        public float reaperDamageMultiplier = 1f;
        public float reaperCritChance = 0f;
        public int deathMarks;
        public const int MAX_DEATH_MARKS = 5;
        public static ModKeybind SoulReleaseKey;
        public bool justConsumedSouls;
        public int TempleBuffTimer;
        private const float BaseMaxSoulEnergy = 100f;
        public bool chillingPresence;

        public override void Load()
        {
            SoulReleaseKey = KeybindLoader.RegisterKeybind(Mod, "Release Soul Energy", "R");
        }

        public override void Unload()
        {
            SoulReleaseKey = null;
        }

        public override void Initialize()
        {
            soulEnergy = 0f;
            soulGatherMultiplier = 1f;
            hasReaperArmor = false;
            reaperDamageMultiplier = 1f;
            reaperCritChance = 0f;
            justConsumedSouls = false;
            deathMarks = 0;
            TempleBuffTimer = 0;
            maxSoulEnergy = BaseMaxSoulEnergy;
            chillingPresence = false;
        }

        public override void ResetEffects()
        {
            maxSoulEnergy = BaseMaxSoulEnergy;
            soulGatherMultiplier = 1f;
            hasReaperArmor = false;
            reaperDamageMultiplier = 1f;
            reaperCritChance = 0f;
            justConsumedSouls = false;
            chillingPresence = false;
            if (TempleBuffTimer > 0)
                TempleBuffTimer--;
        }

        public void AddSoulEnergy(float amount, Vector2 sourcePosition)
        {
            if (amount <= 0f)
                return;

            float adjustedAmount = amount * soulGatherMultiplier;

            if (soulEnergy < maxSoulEnergy)
            {
                soulEnergy = MathHelper.Clamp(soulEnergy + adjustedAmount, 0f, maxSoulEnergy);

                Vector2 dirToPlayer = Player.Center - sourcePosition;
                float distance = dirToPlayer.Length();
                if (distance > 0.01f)
                {
                    dirToPlayer.Normalize();
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 dustPosition = sourcePosition + dirToPlayer * distance * (i / 10f);
                        Dust dust = Dust.NewDustPerfect(
                            dustPosition,
                            DustID.WhiteTorch,
                            dirToPlayer * 5f,
                            0,
                            Color.White,
                            1f
                        );
                        dust.noGravity = true;
                        dust.fadeIn = 1.2f;
                    }
                }
            }
        }

        public void AddSoulEnergy(float amount)
        {
            if (amount <= 0f)
                return;

            soulEnergy = MathHelper.Clamp(soulEnergy + amount * soulGatherMultiplier, 0f, maxSoulEnergy);
        }

        public bool AddSoulEnergyFromNPC(NPC npc, float amount)
        {
            if (npc == null || amount <= 0f)
                return false;

            if (!IsValidSoulSourceNPC(npc))
                return false;

            AddSoulEnergy(amount, npc.Center);
            return true;
        }

        public bool ConsumeSoulEnergy(float amount)
        {
            if (amount <= 0f)
                return true;

            if (soulEnergy >= amount)
            {
                soulEnergy -= amount;
                justConsumedSouls = true;
                return true;
            }
            return false;
        }

        public bool TryReleaseSouls(float cost, Action<Player> onSuccess, string releaseMessage = null)
        {
            if (SoulReleaseKey.JustPressed)
            {
                if (ConsumeSoulEnergy(cost))
                {
                    onSuccess?.Invoke(Player);
                    Console.WriteLine($"Souls consumed: {cost}");
                    Main.NewText(releaseMessage ?? $"{(int)cost} souls released!", Color.Green);
                    return true;
                }
                else
                {
                    Main.NewText("Not enough soul energy to activate!", Color.Red);
                }
            }
            return false;
        }

        public float SoulEnergyPercent => maxSoulEnergy > 0f ? soulEnergy / maxSoulEnergy : 0f;

        public void SetSoulEnergy(float value)
        {
            soulEnergy = MathHelper.Clamp(value, 0f, maxSoulEnergy);
        }

        public float ConsumeAllSouls()
        {
            float consumed = soulEnergy;
            soulEnergy = 0f;
            justConsumedSouls = consumed > 0f;
            return consumed;
        }

        public void UpdateReaperDamageMultiplier(float amount)
        {
            reaperDamageMultiplier = MathHelper.Clamp(reaperDamageMultiplier + amount, 1f, 10f);
        }

        public void UpdateReaperCritChance(float amount)
        {
            reaperCritChance = MathHelper.Clamp(reaperCritChance + amount, 0f, 100f);
        }

        public bool IsValidSoulSourceNPC(NPC npc)
        {
            if (npc == null) return false;
            if (npc.type == NPCID.TargetDummy) return false;
            if (npc.friendly) return false;
            if (npc.townNPC) return false;
            if (npc.dontTakeDamage) return false;
            if (IsStatueSpawned(npc)) return false;
            return true;
        }

        private bool IsStatueSpawned(NPC npc)
        {
            if (npc == null) return false;
            Type t = npc.GetType();
            FieldInfo f = t.GetField("spawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null && f.FieldType == typeof(bool))
            {
                try
                {
                    return (bool)f.GetValue(npc);
                }
                catch { }
            }
            PropertyInfo p = t.GetProperty("spawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.PropertyType == typeof(bool))
            {
                try
                {
                    return (bool)p.GetValue(npc);
                }
                catch { }
            }
            p = t.GetProperty("SpawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.PropertyType == typeof(bool))
            {
                try
                {
                    return (bool)p.GetValue(npc);
                }
                catch { }
            }
            f = t.GetField("SpawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null && f.FieldType == typeof(bool))
            {
                try
                {
                    return (bool)f.GetValue(npc);
                }
                catch { }
            }
            return false;
        }
    }
}