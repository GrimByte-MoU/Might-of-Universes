using System;
using System.Reflection;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ModLoader.IO;

namespace MightofUniverses.Common.Players
{
    public class ReaperPlayer : ModPlayer
    {
        private const float BaseMaxSoulEnergy = 100f;

        public float soulEnergy;
        public float maxSoulEnergy = BaseMaxSoulEnergy;
        public float soulGatherMultiplier = 1f;
        public bool hasReaperArmor;
        public float reaperDamageMultiplier = 1f;
        public float reaperCritChance = 0f;
        public int deathMarks;
        public const int MAX_DEATH_MARKS = 5;
        public static ModKeybind SoulReleaseKey;
        public bool justConsumedSouls;
        public int TempleBuffTimer;
        public bool chillingPresence;

        public override void Load()
        {
            SoulReleaseKey = KeybindLoader.RegisterKeybind(Mod, "Release Soul Energy", "R");
        }
        public override void Unload() => SoulReleaseKey = null;

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
            // Reset; equipment logic will (later) rebuild maxSoulEnergy, but soul gains now use an immediate calc.
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

        // --- NEW: Immediate set bonus scan (independent of tick order) ---
        private float ComputeImmediateSetBonus()
        {
            // Armor indices: 0=head,1=body,2=legs
            int head = Player.armor[0].type;
            int body = Player.armor[1].type;
            int legs = Player.armor[2].type;

            float bonus = 0f;

            bool Eclipse =
                head == ModContent.ItemType<Content.Items.Armors.EclipseHood>() &&
                body == ModContent.ItemType<Content.Items.Armors.EclipseChestplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.EclipseLegwraps>();
            if (Eclipse) bonus += 300f;

            bool Lich =
                head == ModContent.ItemType<Content.Items.Armors.LichMask>() &&
                body == ModContent.ItemType<Content.Items.Armors.LichChestplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.LichGreaves>();
            if (Lich) bonus += 125f;

            bool BoneCollector =
                head == ModContent.ItemType<Content.Items.Armors.BoneCollectorHat>() &&
                body == ModContent.ItemType<Content.Items.Armors.BoneCollectorShirt>() &&
                legs == ModContent.ItemType<Content.Items.Armors.BoneCollectorPants>();
            if (BoneCollector) bonus += 100f;

            bool Gravetender =
                head == ModContent.ItemType<Content.Items.Armors.GravetenderHat>() &&
                body == ModContent.ItemType<Content.Items.Armors.GravetenderChestplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.GravetenderShoes>();
            if (Gravetender) bonus += 40f;

            bool Mortician =
                head == ModContent.ItemType<Content.Items.Armors.MorticianHat>() &&
                body == ModContent.ItemType<Content.Items.Armors.MorticianChestplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.MorticianGreaves>();
            if (Mortician) bonus += 40f;

            bool MurkyIce =
                head == ModContent.ItemType<Content.Items.Armors.MurkyIceHelmet>() &&
                body == ModContent.ItemType<Content.Items.Armors.MurkyIceChestplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.MurkyIceBoots>();
            if (MurkyIce) bonus += 60f;

            bool YinYang =
                head == ModContent.ItemType<Content.Items.Armors.YinYangHat>() &&
                body == ModContent.ItemType<Content.Items.Armors.YinYangCloak>() &&
                legs == ModContent.ItemType<Content.Items.Armors.YinYangShoes>();
            if (YinYang) bonus += 125f;

            bool Orcus =
                head == ModContent.ItemType<Content.Items.Armors.MaskofOrcus>() &&
                body == ModContent.ItemType<Content.Items.Armors.ChestplateofOrcus>() &&
                legs == ModContent.ItemType<Content.Items.Armors.GreavesofOrcus>();
            if (Orcus) bonus += 150f;

            bool MechaCactus =
                head == ModContent.ItemType<Content.Items.Armors.MechaCactusHelmet>() &&
                body == ModContent.ItemType<Content.Items.Armors.MechaCactusBreastplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.MechaCactusLeggings>();
            if (MechaCactus) bonus += 150f;

            bool Chlorotanium =
    head == ModContent.ItemType<Content.Items.Armors.ChlorotaniumMaskedHelmet>() &&
    body == ModContent.ItemType<Content.Items.Armors.ChlorotaniumChestplate>() &&
    legs == ModContent.ItemType<Content.Items.Armors.ChlorotaniumGreaves>();
            if (Chlorotanium) bonus += 200f;

    bool FoundryRevenant =
        head == ModContent.ItemType<Content.Items.Armors.FoundryRevenantVisage>() &&
        body == ModContent.ItemType<Content.Items.Armors.FoundryRevenantBoilerplate>() &&
        legs == ModContent.ItemType<Content.Items.Armors.FoundryRevenantTreads>();
                if (FoundryRevenant) bonus += 250f;

    bool HolidayButcher =
        head == ModContent.ItemType<Content.Items.Armors.HolidayButcherHat>() &&
        body == ModContent.ItemType<Content.Items.Armors.HolidayButcherChestplate>() &&
        legs == ModContent.ItemType<Content.Items.Armors.HolidayButcherGreaves>();
                if (HolidayButcher) bonus += 300f;

    bool PrimalSavagery =
        head == ModContent.ItemType<Content.Items.Armors.PrimalSavageryMask>() &&
        body == ModContent.ItemType<Content.Items.Armors.PrimalSavageryChestplate>() &&
        legs == ModContent.ItemType<Content.Items.Armors.PrimalSavageryBoots>();
    if (PrimalSavagery) bonus += 350f;

    bool TempleReaver =
        head == ModContent.ItemType<Content.Items.Armors.TempleReaverMask>() &&
        body == ModContent.ItemType<Content.Items.Armors.TempleReaverPlate>() &&
        legs == ModContent.ItemType<Content.Items.Armors.TempleReaverBoots>();
    if (TempleReaver) bonus += 350f;

    bool LunarShroud =
        head == ModContent.ItemType<Content.Items.Armors.LunarShroudCowl>() &&
        body == ModContent.ItemType<Content.Items.Armors.LunarShroudCowl>() &&
        legs == ModContent.ItemType<Content.Items.Armors.LunarShroudPants>();
    if (LunarShroud) bonus += 400f;

            return bonus;
        }

        // Public helper if you need it elsewhere
        public float GetEffectiveInstantMax()
        {
            float setBonus = ComputeImmediateSetBonus();
            // Include whatever the current tick has already built into maxSoulEnergy (so we never go lower)
            float baseline = Math.Max(maxSoulEnergy, BaseMaxSoulEnergy + setBonus);
            return baseline;
        }

        private float GetClampMaxForGain()
        {
            // The immediate set-based max might exceed current maxSoulEnergy early in the tick.
            float immediate = BaseMaxSoulEnergy + ComputeImmediateSetBonus();
            return Math.Max(immediate, maxSoulEnergy);
        }

        public void AddSoulEnergy(float amount, Vector2 sourcePosition)
        {
            if (amount <= 0f) return;

            float clampMax = GetClampMaxForGain();
            float adjusted = amount * soulGatherMultiplier;

            if (soulEnergy < clampMax)
            {
                soulEnergy = MathHelper.Clamp(soulEnergy + adjusted, 0f, clampMax);

                Vector2 dir = Player.Center - sourcePosition;
                float dist = dir.Length();
                if (dist > 0.01f)
                {
                    dir.Normalize();
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 dustPos = sourcePosition + dir * dist * (i / 10f);
                        var d = Dust.NewDustPerfect(dustPos, DustID.WhiteTorch, dir * 5f, 0, Color.White, 1f);
                        d.noGravity = true;
                        d.fadeIn = 1.2f;
                    }
                }
            }
        }

        public void AddSoulEnergy(float amount)
        {
            if (amount <= 0f) return;
            float clampMax = GetClampMaxForGain();
            soulEnergy = MathHelper.Clamp(soulEnergy + amount * soulGatherMultiplier, 0f, clampMax);
        }

        public bool AddSoulEnergyFromNPC(NPC npc, float amount)
        {
            if (npc == null || amount <= 0f) return false;
            if (!IsValidSoulSourceNPC(npc)) return false;
            AddSoulEnergy(amount, npc.Center);
            return true;
        }

        public bool ConsumeSoulEnergy(float amount)
        {
            if (amount <= 0f) return true;
            if (soulEnergy >= amount)
            {
                soulEnergy -= amount;
                justConsumedSouls = true;
                return true;
            }
            return false;
        }

        public bool TryReleaseSouls(float cost, Action<Player> onSuccess, string msg = null)
        {
            if (SoulReleaseKey.JustPressed)
            {
                if (ConsumeSoulEnergy(cost))
                {
                    onSuccess?.Invoke(Player);
                    Main.NewText(msg ?? $"{(int)cost} souls released!", Color.Green);
                    return true;
                }
                Main.NewText("Not enough soul energy to activate!", Color.Red);
            }
            return false;
        }

        public float SoulEnergyPercent
        {
            get
            {
                float clampMax = GetClampMaxForGain();
                return clampMax > 0f ? soulEnergy / clampMax : 0f;
            }
        }

        public void SetSoulEnergy(float value)
        {
            float clampMax = GetClampMaxForGain();
            soulEnergy = MathHelper.Clamp(value, 0f, clampMax);
        }

        public float ConsumeAllSouls()
        {
            float taken = soulEnergy;
            soulEnergy = 0f;
            justConsumedSouls = taken > 0f;
            return taken;
        }

        public override void PostUpdate()
        {
            float setImmediate = BaseMaxSoulEnergy + ComputeImmediateSetBonus();
            if (maxSoulEnergy < setImmediate)
                maxSoulEnergy = setImmediate;

            if (soulEnergy > maxSoulEnergy)
                soulEnergy = maxSoulEnergy;
        }

        public void UpdateReaperDamageMultiplier(float amt) =>
            reaperDamageMultiplier = MathHelper.Clamp(reaperDamageMultiplier + amt, 1f, 10f);

        public void UpdateReaperCritChance(float amt) =>
            reaperCritChance = MathHelper.Clamp(reaperCritChance + amt, 0f, 100f);

        public bool IsValidSoulSourceNPC(NPC npc)
        {
            if (npc == null) return false;
            if (npc.type == NPCID.TargetDummy) return false;
            if (npc.friendly || npc.townNPC || npc.dontTakeDamage) return false;
            if (IsStatueSpawned(npc)) return false;
            return true;
        }

        private bool IsStatueSpawned(NPC npc)
        {
            if (npc == null) return false;
            Type t = npc.GetType();

            FieldInfo f = t.GetField("spawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null && f.FieldType == typeof(bool))
                try { return (bool)f.GetValue(npc); } catch { }

            PropertyInfo p = t.GetProperty("spawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.PropertyType == typeof(bool))
                try { return (bool)p.GetValue(npc); } catch { }

            p = t.GetProperty("SpawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.PropertyType == typeof(bool))
                try { return (bool)p.GetValue(npc); } catch { }

            f = t.GetField("SpawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null && f.FieldType == typeof(bool))
                try { return (bool)f.GetValue(npc); } catch { }

            return false;
        }

        // --- Death mark helpers / persistence & networking ---

        // Adds up to MAX_DEATH_MARKS and syncs to clients (server authoritative)
        public void AddDeathMarks(int amount)
        {
            if (amount <= 0) return;
            int old = deathMarks;
            deathMarks = Math.Min(MAX_DEATH_MARKS, deathMarks + amount);
            if (deathMarks != old)
            {
                if (Main.netMode == NetmodeID.Server)
                    SyncDeathMarks();
            }
        }

        // Consume a number of marks. Returns true if successful.
        public bool ConsumeDeathMarks(int amount)
        {
            if (amount <= 0) return true;
            if (deathMarks >= amount)
            {
                deathMarks -= amount;
                if (Main.netMode == NetmodeID.Server)
                    SyncDeathMarks();
                return true;
            }
            return false;
        }

        // Force clear (e.g., on death)
        public void ForceClearDeathMarks()
        {
            if (deathMarks == 0) return;
            deathMarks = 0;
            if (Main.netMode == NetmodeID.Server)
                SyncDeathMarks();
        }

        // Server -> clients quick sync helper (send a small packet with marks)
        public void SyncDeathMarks()
        {
            if (Main.netMode != NetmodeID.Server) return;
            var packet = Mod.GetPacket();
            packet.Write((byte)1); // opcode 1 = DeathMarks sync packet (handle in Mod.HandlePacket)
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)deathMarks);
            packet.Send();
        }

        public override void SaveData(TagCompound tag)
        {
            tag["DeathMarks"] = deathMarks;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("DeathMarks"))
                deathMarks = tag.GetInt("DeathMarks");
        }

        public void NetSend(BinaryWriter writer)
        {
            writer.Write((int)deathMarks);
        }

        public void NetReceive(BinaryReader reader)
        {
            deathMarks = reader.ReadInt32();
        }

        public enum SoulReleaseResult
        {
            Success,
            Failure
        }
    }
}