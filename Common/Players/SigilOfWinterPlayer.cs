using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using MightofUniverses.Common.Input;


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
        sigilDuration = 300; // 5 seconds
        sigilCooldown = 2700; // 45 seconds
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
                Player.moveSpeed *= 0.1f;
                Player.maxRunSpeed *= 0.1f;
                Player.endurance += 0.25f;
                Player.statDefense += 30;

                if (sigilDuration == 0)
                {
                    if (Player.hurtCooldowns[0] == 0 && Player.hurtCooldowns[1] == 0)
{
    Player.statLife += 50;

    for (int i = 0; i < Player.buffType.Length; i++)
    {
        int buffType = Player.buffType[i];
        if (Main.debuff[buffType])
        {
            Player.DelBuff(i);
            i--; // Maintain loop integrity
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
    }
}
