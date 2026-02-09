using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Common.Players
{
    public class MOUPlayer : ModPlayer
    {
        public bool EquippedSkyRibbon;
        public bool EquippedOverclockRemote;
        public float MinionMoveSpeedBonus;
        public int OverclockActiveTimer;
        public int OverclockLockoutTimer;
        public int OverclockCooldown;

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
            if (Content.KeybindsSystem.OverclockKeybind.JustPressed && EquippedOverclockRemote && OverclockCooldown <= 0)
            {
                OverclockActiveTimer = 120;
                OverclockLockoutTimer = 60;
                OverclockCooldown = 900;

                if (Main.myPlayer == Player.whoAmI)
                {
                    CombatText.NewText(Player.getRect(), Microsoft.Xna.Framework.Color.Orange,
                        "Overclocked!", dramatic: true);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, Player.Center);
                }
            }
        }

        public override void PostUpdateEquips()
        {
            if (EquippedSkyRibbon)
                Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 1.45f;

            if (IsOverclockActive && EquippedOverclockRemote)
                Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 2.0f;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!(IsOverclockActive && EquippedOverclockRemote))
                return;
            bool isWhip = ProjectileID.Sets.IsAWhip[proj.type];
            bool isMinionDamage = proj.minion || proj.DamageType == DamageClass.Summon;

            if (isMinionDamage && !isWhip)
            {
                modifiers.SourceDamage *= 2f;
            }
        }
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