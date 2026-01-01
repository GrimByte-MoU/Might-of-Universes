using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using MightofUniverses.Common.Input;
using Terraria.ID;
using System;

namespace MightofUniverses.Common.Players
{
    public class SigilOfWinterPlayer : ModPlayer
    {
        public bool hasSigilOfWinter = false;
        private int sigilCooldown = 0;
        private int sigilDuration = 0;
        private bool sigilActive = false;

        public override void ResetEffects()
        {
            hasSigilOfWinter = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (hasSigilOfWinter && sigilCooldown <= 0 && ModKeybindManager.Ability2.JustPressed)
            {
                sigilActive = true;
                sigilDuration = 300;
                sigilCooldown = 2700;
                CombatText.NewText(Player.Hitbox, Color.Aqua, "Sigil Activated");
            }
        }

        public override void PostUpdate()
        {
            if (sigilCooldown > 0)
                sigilCooldown--;

            if (sigilActive && sigilDuration > 0)
            {
                sigilDuration--;
                float speedLimit = 1.5f;
                if (Player.velocity.Length() > speedLimit)
                Player.velocity = Player.velocity.SafeNormalize(Vector2.Zero) * speedLimit;
                Player.runAcceleration *= 0.1f;
                Player.jumpSpeedBoost *= 0.1f;
                Player.maxRunSpeed *= 0.1f;
                Player.moveSpeed *= 0.1f;
                Player.endurance += 0.25f;
                Player.statDefense += 30;
                Player.controlHook = false;
                Player.controlJump = false;
                Player.controlMount = false;
                Player.gravControl = false;
                if (Player.whoAmI == Main.myPlayer)
                {
                    int dustCount = 24;
                    float radius = Player.width * 0.8f;
                    for (int i = 0; i < dustCount; i++)
                    {
                        float angle = MathHelper.TwoPi * i / dustCount;
                        Vector2 pos = Player.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                        int dust = Dust.NewDust(pos, 0, 0, DustID.Ice, 0f, 0f, 120, Color.Aqua, 1.2f);
                        Main.dust[dust].noGravity = true;
                    }
                }
                if (sigilDuration == 0)
                {
                    if (Player.hurtCooldowns[0] == 0 && Player.hurtCooldowns[1] == 0)
                    {
                        Player.statLife += 50;
                        if (Player.statLife > Player.statLifeMax2)
                            Player.statLife = Player.statLifeMax2;

                        // Remove all debuffs
                        for (int i = 0; i < Player.buffType.Length; i++)
                        {
                            int buffType = Player.buffType[i];
                            if (buffType > 0 && Main.debuff[buffType])
                            {
                                Player.DelBuff(i);
                                i--;
                            }
                        }
                        CombatText.NewText(Player.Hitbox, Color.LightBlue, "+50 HP, Debuffs Cleared");
                    }
                    sigilActive = false;
                }
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (sigilActive)
            {
                sigilActive = false;
                sigilDuration = 0;
                CombatText.NewText(Player.Hitbox, Color.Gray, "Sigil Interrupted");
            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (sigilActive)
            {
                sigilActive = false;
                sigilDuration = 0;
                CombatText.NewText(Player.Hitbox, Color.Gray, "Sigil Interrupted");
            }
        }
        public int SigilCooldown => sigilCooldown;
        public int SigilDuration => sigilDuration;
        public bool SigilIsActive => sigilActive;
    }
}