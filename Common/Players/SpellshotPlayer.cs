using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Accessories;
using Microsoft.Xna.Framework;

public class SpellshotPlayer : ModPlayer
{
    public bool spellshotEffect;

    public override void ResetEffects()
    {
        spellshotEffect = false;
    }

    public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (spellshotEffect)
        {
            if (item.DamageType == DamageClass.Ranged)
            {
                int manaCost = item.useTime;
                if (Player.CheckMana(manaCost, true))
                {
                    damage += manaCost;
                    Player.statMana -= manaCost;
                }
            }
            else if (item.DamageType == DamageClass.Magic && Player.PickAmmo(item, out int projToShoot, out float speed, out int damage2, out float knockback2, out int ammoItemId))
            {
                damage += damage2;
            }
        }
    }
}
