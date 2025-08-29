using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Accessories;
using Microsoft.Xna.Framework;

public class CommanderPlayer : ModPlayer
{
    public bool commanderEffect;

    public override void ResetEffects()
    {
        commanderEffect = false;
    }

    public override void UpdateEquips()
    {
        if (commanderEffect)
        {
            Player.GetAttackSpeed(DamageClass.Ranged) += Player.numMinions * 0.05f;
        }
    }

    public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (commanderEffect && proj.DamageType == DamageClass.Summon && Player.statLife <= Player.statLifeMax2 * 0.5f)
        {
            Item ammo = new Item();
            ammo.SetDefaults(Player.inventory[Player.selectedItem].useAmmo);
            modifiers.SourceDamage *= 1f;
        }
    }
}
