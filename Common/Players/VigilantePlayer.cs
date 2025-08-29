using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Accessories;
using Microsoft.Xna.Framework;

public class VigilantePlayer : ModPlayer
{
    public bool vigilanteEffect;

    public override void ResetEffects()
    {
        vigilanteEffect = false;
    }

    public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (!vigilanteEffect) return;

        float distance = Vector2.Distance(target.Center, Player.Center) / 16f;
        
        if (proj.DamageType == DamageClass.Melee)
        {
            float bonus = Math.Min(distance / 20f, 0.2f);
            modifiers.SourceDamage += bonus;
        }
        else if (proj.DamageType == DamageClass.Ranged)
        {
            float bonus = Math.Max(0.2f - (distance / 25f), 0f);
            modifiers.SourceDamage += bonus;
        }
    }
}
