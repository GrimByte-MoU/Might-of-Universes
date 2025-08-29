using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Accessories;
using Microsoft.Xna.Framework;

public class ConjurerPlayer : ModPlayer
{
    public bool conjurerEffect;

    public override void ResetEffects()
    {
        conjurerEffect = false;
    }

    public override void ModifyWeaponCrit(Item item, ref float crit)
    {
        if (conjurerEffect && item.DamageType == DamageClass.Magic)
        {
            crit += Player.numMinions * 2;
        }
    }

    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (conjurerEffect && proj.DamageType == DamageClass.Summon)
        {
            Player.statMana = Math.Min(Player.statMana + 5, Player.statManaMax2);
        }
    }
}
