using System;
using System. Reflection;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria. GameInput;
using Terraria.ModLoader.IO;
using Terraria.Audio;

namespace MightofUniverses.Common.Players
{
    public class ReaperPlayer : ModPlayer
    {
        private const float BaseMaxSoulEnergy = 100f;
        
        public float soulEnergy;
        public float maxSoulEnergy = BaseMaxSoulEnergy;
        public float soulGatherMultiplier = 1f;
        public bool hasReaperArmor;
        public float reaperDamageMultiplier = 0f;
        public float reaperCritChance = 0f;
        public int deathMarks;
        public const int MAX_DEATH_MARKS = 5;
        
        private int outOfCombatTimer = 0;
        private const int OUT_OF_COMBAT_THRESHOLD = 300;
        private const int DEATH_MARK_DECAY_RATE = 1800;
        private int lastHitTime = 0;
        
        public static ModKeybind SoulReleaseKey;
        public static ModKeybind Ability2Key;
        
        public bool hasUnstableCharm = false;
        public bool hasMortalityBell = false;
        public bool hasCultistTapestry = false;
        
        private int mortalityBellCooldown = 0;
        private const int MORTALITY_BELL_COOLDOWN = 3600;
        
        public bool justConsumedSouls;
        public int TempleBuffTimer;
        public bool chillingPresence;
        public float lastSoulsConsumed = 0f;

        public override void Load()
        {
            SoulReleaseKey = KeybindLoader.RegisterKeybind(Mod, "Release Soul Energy", "R");
            Ability2Key = KeybindLoader.RegisterKeybind(Mod, "Ability 2", "F");
        }

        public override void Unload()
        {
            SoulReleaseKey = null;
            Ability2Key = null;
        }

        public override void Initialize()
        {
            soulEnergy = 0f;
            maxSoulEnergy = BaseMaxSoulEnergy;
            soulGatherMultiplier = 1f;
            hasReaperArmor = false;
            reaperDamageMultiplier = 0f;
            reaperCritChance = 0f;
            deathMarks = 0;
            outOfCombatTimer = 0;
            lastHitTime = 0;
            hasUnstableCharm = false;
            hasMortalityBell = false;
            hasCultistTapestry = false;
            mortalityBellCooldown = 0;
            justConsumedSouls = false;
            TempleBuffTimer = 0;
            chillingPresence = false;
            lastSoulsConsumed = 0f;
        }

        public override void ResetEffects()
        {
            maxSoulEnergy = BaseMaxSoulEnergy;
            soulGatherMultiplier = 1f;
            hasReaperArmor = false;
            reaperDamageMultiplier = 0f;
            reaperCritChance = 0f;
            justConsumedSouls = false;
            chillingPresence = false;
            hasUnstableCharm = false;
            hasMortalityBell = false;
            hasCultistTapestry = false;
            lastSoulsConsumed = 0f;
            
            if (TempleBuffTimer > 0)
                TempleBuffTimer--;
        }

        public override void PostUpdate()
        {
            float armorSetBonus = ComputeArmorSetBonus();
            float effectiveMax = BaseMaxSoulEnergy + armorSetBonus;
            
            if (maxSoulEnergy < effectiveMax)
                maxSoulEnergy = effectiveMax;
            
            if (soulEnergy > maxSoulEnergy)
                soulEnergy = maxSoulEnergy;
            
            if (deathMarks > 0)
            {
                lastHitTime++;

                if (lastHitTime > OUT_OF_COMBAT_THRESHOLD)
                {
                    outOfCombatTimer++;

                    if (outOfCombatTimer >= DEATH_MARK_DECAY_RATE)
                    {
                        outOfCombatTimer = 0;
                        deathMarks = Math.Max(0, deathMarks - 1);
                        
                        if (Main.myPlayer == Player.whoAmI)
                        {
                            CombatText.NewText(Player.getRect(), Color.Gray, "-1 Death Mark (Decay)", true);
                        }

                        if (hasCultistTapestry)
                        {
                            Player. Heal(50);
                            float soulGain = maxSoulEnergy * 0.20f;
                            AddSoulEnergy(soulGain);
                            SoundEngine.PlaySound(SoundID.Item4, Player.Center);
                            
                            for (int i = 0; i < 20; i++)
                            {
                                Vector2 dustVel = Main.rand.NextVector2Circular(5f, 5f);
                                Dust dust = Dust. NewDustDirect(Player. position, Player.width, Player. height, DustID.GreenTorch, dustVel. X, dustVel.Y, 100, Color. Lime, 1.8f);
                                dust.noGravity = true;
                            }

                            CombatText.NewText(Player.getRect(), Color.Lime, $"+50 HP, +{(int)soulGain} Souls", true);
                        }

                        if (Main.netMode == NetmodeID.Server)
                            SyncDeathMarks();
                    }
                }
                else
                {
                    outOfCombatTimer = 0;
                }
            }
            else
            {
                outOfCombatTimer = 0;
                lastHitTime = 0;
            }

            if (mortalityBellCooldown > 0)
            {
                mortalityBellCooldown--;
            }

            if (hasUnstableCharm && Ability2Key != null && Ability2Key.JustPressed)
            {
                TryUnstableCharmTeleport();
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            lastHitTime = 0;

            if (proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                modifiers.SourceDamage *= (1f + reaperDamageMultiplier);
                modifiers.CritDamage += reaperCritChance / 100f;
            }
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC. HitModifiers modifiers)
        {
            lastHitTime = 0;

            if (item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                modifiers.SourceDamage *= (1f + reaperDamageMultiplier);
                modifiers.CritDamage += reaperCritChance / 100f;
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            lastHitTime = 0;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            lastHitTime = 0;
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            lastHitTime = 0;
        }

        public void AddSoulEnergy(float amount, Vector2 sourcePosition)
        {
            if (amount <= 0f) return;

            float clampMax = GetEffectiveMax();
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
                        Dust d = Dust.NewDustPerfect(dustPos, DustID.WhiteTorch, dir * 5f, 0, Color.White, 1f);
                        d.noGravity = true;
                        d.fadeIn = 1.2f;
                    }
                }
            }
        }

        public void AddSoulEnergy(float amount)
        {
            if (amount <= 0f) return;
            
            float clampMax = GetEffectiveMax();
            soulEnergy = MathHelper. Clamp(soulEnergy + (amount * soulGatherMultiplier), 0f, clampMax);
        }

        public bool AddSoulEnergyFromNPC(NPC npc, float amount)
        {
            if (npc == null || amount <= 0f) return false;
            if (! IsValidSoulSourceNPC(npc)) return false;
            
            AddSoulEnergy(amount, npc.Center);
            return true;
        }

        public bool ConsumeSoulEnergy(float amount)
        {
            if (amount <= 0f) return true;
            
            if (soulEnergy >= amount)
            {
                soulEnergy -= amount;
                lastSoulsConsumed = amount;
                justConsumedSouls = true;
                return true;
            }
            
            return false;
        }

        public bool TryReleaseSouls(float cost, Action<Player> onSuccess, string msg = null)
        {
            if (SoulReleaseKey. JustPressed)
            {
                if (ConsumeSoulEnergy(cost))
                {
                    onSuccess?. Invoke(Player);
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
                float max = GetEffectiveMax();
                return max > 0f ? soulEnergy / max : 0f;
            }
        }

        public void SetSoulEnergy(float value)
        {
            float clampMax = GetEffectiveMax();
            soulEnergy = MathHelper.Clamp(value, 0f, clampMax);
        }

        public float ConsumeAllSouls()
        {
            float taken = soulEnergy;
            soulEnergy = 0f;
            lastSoulsConsumed = taken;
            justConsumedSouls = taken > 0f;
            return taken;
        }

        private float GetEffectiveMax()
        {
            float armorBonus = ComputeArmorSetBonus();
            float effectiveMax = BaseMaxSoulEnergy + armorBonus;
            return Math.Max(effectiveMax, maxSoulEnergy);
        }

        private float ComputeArmorSetBonus()
        {
            int head = Player.armor[0]. type;
            int body = Player.armor[1].type;
            int legs = Player. armor[2].type;

            float bonus = 0f;

            if (head == ModContent.ItemType<Content.Items.Armors.EclipseHood>() &&
                body == ModContent.ItemType<Content.Items.Armors.EclipseChestplate>() &&
                legs == ModContent.ItemType<Content. Items.Armors.EclipseLegwraps>())
                bonus += 300f;

            if (head == ModContent.ItemType<Content.Items.Armors.LichMask>() &&
                body == ModContent.ItemType<Content.Items.Armors.LichChestplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.LichGreaves>())
                bonus += 125f;

            if (head == ModContent.ItemType<Content. Items.Armors.BoneCollectorHat>() &&
                body == ModContent.ItemType<Content.Items.Armors.BoneCollectorShirt>() &&
                legs == ModContent.ItemType<Content. Items.Armors.BoneCollectorPants>())
                bonus += 100f;

            if (head == ModContent.ItemType<Content. Items.Armors.GravetenderHat>() &&
                body == ModContent.ItemType<Content.Items.Armors. GravetenderChestplate>() &&
                legs == ModContent.ItemType<Content. Items.Armors.GravetenderShoes>())
                bonus += 40f;

            if (head == ModContent.ItemType<Content. Items.Armors.MorticianHat>() &&
                body == ModContent.ItemType<Content.Items.Armors. MorticianChestplate>() &&
                legs == ModContent.ItemType<Content.Items. Armors.MorticianGreaves>())
                bonus += 40f;

            if (head == ModContent.ItemType<Content. Items.Armors.MurkyIceHelmet>() &&
                body == ModContent.ItemType<Content.Items.Armors.MurkyIceChestplate>() &&
                legs == ModContent.ItemType<Content.Items. Armors.MurkyIceBoots>())
                bonus += 60f;

            if (head == ModContent.ItemType<Content. Items.Armors.YinYangHat>() &&
                body == ModContent.ItemType<Content.Items.Armors.YinYangCloak>() &&
                legs == ModContent.ItemType<Content. Items.Armors.YinYangShoes>())
                bonus += 125f;

            if (head == ModContent.ItemType<Content. Items.Armors.MaskofOrcus>() &&
                body == ModContent.ItemType<Content.Items.Armors.ChestplateofOrcus>() &&
                legs == ModContent. ItemType<Content.Items. Armors. GreavesofOrcus>())
                bonus += 150f;

            if (head == ModContent.ItemType<Content. Items.Armors.MechaCactusHelmet>() &&
                body == ModContent.ItemType<Content. Items.Armors.MechaCactusBreastplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.MechaCactusLeggings>())
                bonus += 150f;

            if (head == ModContent.ItemType<Content. Items.Armors.ChlorotaniumMaskedHelmet>() &&
                body == ModContent.ItemType<Content.Items.Armors.ChlorotaniumChestplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.ChlorotaniumGreaves>())
                bonus += 200f;

            if (head == ModContent.ItemType<Content.Items.Armors. FoundryRevenantVisage>() &&
                body == ModContent.ItemType<Content. Items.Armors.FoundryRevenantBoilerplate>() &&
                legs == ModContent.ItemType<Content. Items.Armors.FoundryRevenantTreads>())
                bonus += 250f;

            if (head == ModContent.ItemType<Content. Items.Armors.HolidayButcherHat>() &&
                body == ModContent.ItemType<Content. Items.Armors.HolidayButcherChestplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.HolidayButcherGreaves>())
                bonus += 300f;

            if (head == ModContent.ItemType<Content. Items.Armors.PrimalSavageryMask>() &&
                body == ModContent.ItemType<Content. Items.Armors.PrimalSavageryChestplate>() &&
                legs == ModContent.ItemType<Content.Items.Armors.PrimalSavageryBoots>())
                bonus += 350f;

            if (head == ModContent.ItemType<Content. Items.Armors. TempleReaverMask>() &&
                body == ModContent.ItemType<Content.Items.Armors. TempleReaverPlate>() &&
                legs == ModContent.ItemType<Content. Items.Armors.TempleReaverBoots>())
                bonus += 350f;

            if (head == ModContent.ItemType<Content. Items.Armors.LunarShroudCowl>() &&
                body == ModContent.ItemType<Content.Items.Armors.LunarShroudCowl>() &&
                legs == ModContent.ItemType<Content.Items. Armors.LunarShroudPants>())
                bonus += 400f;

            return bonus;
        }

        public void UpdateReaperDamageMultiplier(float amt)
        {
            reaperDamageMultiplier = MathHelper.Clamp(reaperDamageMultiplier + amt, 0f, 10f);
        }

        public void UpdateReaperCritChance(float amt)
        {
            reaperCritChance = MathHelper.Clamp(reaperCritChance + amt, 0f, 100f);
        }

        public bool IsValidSoulSourceNPC(NPC npc)
        {
            if (npc == null) return false;
            if (npc. type == NPCID. TargetDummy) return false;
            if (npc.friendly || npc.townNPC || npc. dontTakeDamage) return false;
            if (IsStatueSpawned(npc)) return false;
            
            return true;
        }

        private bool IsStatueSpawned(NPC npc)
        {
            if (npc == null) return false;
            
            Type t = npc.GetType();

            FieldInfo f = t.GetField("spawnedFromStatue", BindingFlags. Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null && f. FieldType == typeof(bool))
            {
                try { return (bool)f.GetValue(npc); } 
                catch { }
            }

            PropertyInfo p = t.GetProperty("spawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.PropertyType == typeof(bool))
            {
                try { return (bool)p.GetValue(npc); } 
                catch { }
            }

            p = t.GetProperty("SpawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.PropertyType == typeof(bool))
            {
                try { return (bool)p.GetValue(npc); } 
                catch { }
            }

            f = t.GetField("SpawnedFromStatue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null && f. FieldType == typeof(bool))
            {
                try { return (bool)f.GetValue(npc); } 
                catch { }
            }

            return false;
        }

        public void AddDeathMarks(int amount)
        {
            if (amount <= 0) return;
            
            int old = deathMarks;
            deathMarks = Math.Min(MAX_DEATH_MARKS, deathMarks + amount);
            
            if (deathMarks != old && Main.netMode == NetmodeID.Server)
                SyncDeathMarks();
        }

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

        public void RemoveDeathMark(int amount)
        {
            if (amount <= 0) return;
            
            deathMarks = Math.Max(0, deathMarks - amount);
            
            if (Main.netMode == NetmodeID.Server)
                SyncDeathMarks();
        }

        public void ForceClearDeathMarks()
        {
            if (deathMarks == 0) return;
            
            deathMarks = 0;
            
            if (Main.netMode == NetmodeID.Server)
                SyncDeathMarks();
        }

        public void SyncDeathMarks()
        {
            if (Main.netMode != NetmodeID.Server) return;
            
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)1);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)deathMarks);
            packet.Send();
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (deathMarks > 0)
            {
                ForceClearDeathMarks();
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (hasMortalityBell && mortalityBellCooldown <= 0 && deathMarks > 0)
            {
                int healAmount = (int)(Player.statLifeMax2 * 0.10f * deathMarks);
                int marksConsumed = deathMarks;

                ForceClearDeathMarks();

                Player. Heal(healAmount);
                Player.immune = true;
                Player.immuneTime = 120;

                mortalityBellCooldown = MORTALITY_BELL_COOLDOWN;

                SoundEngine.PlaySound(SoundID.Item4, Player.Center);
                
                for (int i = 0; i < 50; i++)
                {
                    Vector2 dustVel = Main.rand.NextVector2Circular(8f, 8f);
                    Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID. LifeCrystal, dustVel.X, dustVel.Y, 100, Color.LightGreen, 2f);
                    dust.noGravity = true;
                }

                CombatText.NewText(Player.getRect(), Color.LightGreen, $"Saved by {marksConsumed} Death Marks!", true);

                return false;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        private void TryUnstableCharmTeleport()
        {
            if (deathMarks <= 0) return;

            Vector2 cursorDirection = Main.MouseWorld - Player.Center;
            
            if (cursorDirection.LengthSquared() < 0.0001f)
                cursorDirection = new Vector2(Player.direction, 0f);
            
            cursorDirection. Normalize();

            Vector2 teleportOffset = cursorDirection * (15f * 16f);
            Vector2 newPosition = Player.Center + teleportOffset;

            Point tilePos = newPosition.ToTileCoordinates();
            
            if (WorldGen.InWorld(tilePos. X, tilePos.Y))
            {
                RemoveDeathMark(1);

                Player.position = newPosition - new Vector2(Player.width / 2f, Player.height / 2f);
                Player.velocity = Vector2.Zero;

                SoundEngine.PlaySound(SoundID.Item6, Player.Center);

                for (int i = 0; i < 30; i++)
                {
                    Vector2 oldDustPos = Player.Center - teleportOffset;
                    Dust d1 = Dust.NewDustDirect(oldDustPos + Main.rand.NextVector2Circular(20f, 20f), 4, 4, DustID.MagicMirror, 0, 0, 100, Color.Purple, 1.5f);
                    d1.noGravity = true;
                    d1.velocity = Main.rand.NextVector2Circular(3f, 3f);

                    Dust d2 = Dust.NewDustDirect(Player.Center + Main.rand.NextVector2Circular(20f, 20f), 4, 4, DustID.MagicMirror, 0, 0, 100, Color.Purple, 1.5f);
                    d2.noGravity = true;
                    d2.velocity = Main.rand.NextVector2Circular(3f, 3f);
                }

                Player.immune = true;
                Player.immuneTime = 20;
            }
        }

        public override void SaveData(TagCompound tag)
        {
        }

        public override void LoadData(TagCompound tag)
        {
            deathMarks = 0;
        }

        public void NetSend(BinaryWriter writer)
        {
            writer.Write(deathMarks);
        }

        public void NetReceive(BinaryReader reader)
        {
            deathMarks = reader. ReadInt32();
        }
    }
}