using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Accessories;
using Microsoft.Xna.Framework;

public class WarcallerPlayer : ModPlayer
{
    public bool warcallerEffect;

    public override void ResetEffects()
    {
        warcallerEffect = false;
    }

    public override void UpdateEquips()
    {
        if (warcallerEffect)
        {
            Player.statDefense += Player.numMinions * 2;
        }
    }

    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (warcallerEffect && item.DamageType == DamageClass.Melee)
        {
            Player.MinionAttackTargetNPC = target.whoAmI;
        }
    }
}
