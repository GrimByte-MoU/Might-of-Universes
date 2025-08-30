using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Common.Players
{
    // Shared summoner stats and Overclocking Remote logic
    public class MOUPlayer : ModPlayer
    {
        // Equip flags
        public bool EquippedSkyRibbon;
        public bool EquippedOverclockRemote;

        // Minion movement speed bonus (additive; e.g., 0.20f = +20%)
        public float MinionMoveSpeedBonus;

        // Overclock timers (in ticks)
        // Active: +100% whip speed and 2x minion damage
        // Lockout: minion attacks suppressed (whips unaffected)
        // Cooldown: cannot re-activate until 15s passes
        public int OverclockActiveTimer;   // 120 ticks (2s)
        public int OverclockLockoutTimer;  // 60 ticks (1s)
        public int OverclockCooldown;      // 900 ticks (15s)

        public bool IsOverclockActive => OverclockActiveTimer > 0;
        public bool IsOverclockLockout => OverclockLockoutTimer > 0;

        public override void ResetEffects()
        {
            EquippedSkyRibbon = false;
            EquippedOverclockRemote = false;
            MinionMoveSpeedBonus = 0f;
        }

        public override void PostUpdate()
        {
            if (OverclockActiveTimer > 0) OverclockActiveTimer--;
            else if (OverclockLockoutTimer > 0) OverclockLockoutTimer--;
            if (OverclockCooldown > 0) OverclockCooldown--;
        }

        public override void ProcessTriggers(Terraria.GameInput.TriggersSet triggersSet)
        {
            // Swap this keybind source to your own if needed (e.g., ModKeybindManager.Ability1)
            if (Content.KeybindsSystem.OverclockKeybind.JustPressed && EquippedOverclockRemote && OverclockCooldown <= 0)
            {
                // Start: 2s buff, then 1s lockout, 15s cooldown
                OverclockActiveTimer = 120;
                OverclockLockoutTimer = 60; // will tick after active ends
                OverclockCooldown = 900;

                if (Main.myPlayer == Player.whoAmI)
                {
                    CombatText.NewText(Player.getRect(), Microsoft.Xna.Framework.Color.Orange,
                        "Overclocked!", dramatic: true);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, Player.Center);
                }
            }
        }

        // Applies whip attack speed changes (Sky Ribbon and Overclock)
        public override void PostUpdateEquips()
        {
            if (EquippedSkyRibbon)
                Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 1.45f;

            if (IsOverclockActive && EquippedOverclockRemote)
                Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 2.0f;
        }

        // Double minion damage while Overclock is active (does not affect whips)
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!(IsOverclockActive && EquippedOverclockRemote))
                return;

            // Only minion damage, not whips
            bool isWhip = ProjectileID.Sets.IsAWhip[proj.type];
            bool isMinionDamage = proj.minion || proj.DamageType == DamageClass.Summon;

            if (isMinionDamage && !isWhip)
            {
                // Multiply base (source) damage by 2x
                modifiers.SourceDamage *= 2f;
            }
        }

        // Helper flags for your minion AI
        public bool MinionAttacksSuppressed => IsOverclockLockout && EquippedOverclockRemote;
        public float GetMinionMoveSpeedMult() => 1f + MinionMoveSpeedBonus;
        public float GetOverclockMinionDamageMult() => (IsOverclockActive && EquippedOverclockRemote) ? 2f : 1f;
    }
}

namespace MightofUniverses.Content
{
    public class KeybindsSystem : ModSystem
    {
        public static ModKeybind OverclockKeybind;

        public override void Load()
        {
            OverclockKeybind = KeybindLoader.RegisterKeybind(Mod, "Overclock Remote", "O");
        }

        public override void Unload()
        {
            OverclockKeybind = null;
        }
    }
}