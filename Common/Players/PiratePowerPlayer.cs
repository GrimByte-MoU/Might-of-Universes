using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.Audio;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class PiratePowerPlayer : ModPlayer
    {
        public bool hasPiratePower = false;
private bool wasHitDuringBuff = false;

public override void ResetEffects()
{
    hasPiratePower = false;
}

public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
{
    if (hasPiratePower)
    {
        int goldHeld = CountGoldCoinsInInventory();
        if (goldHeld > 20) goldHeld = 20;

        float bonusDamage = goldHeld * 0.01f;
        float bonusCritChance = goldHeld * 0.5f;
        float bonusCritDamage = goldHeld * 0.025f;

        modifiers.FlatBonusDamage += (int)(Player.GetTotalDamage(DamageClass.Generic).ApplyTo(10) * bonusDamage);
        Player.GetCritChance(DamageClass.Generic) += bonusCritChance;
        Player.GetModPlayer<CritDamagePlayer>().bonusCritMultiplier += bonusCritDamage;
    }
}

public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
{
    if (hasPiratePower && !wasHitDuringBuff)
    {
        wasHitDuringBuff = true;
        Player.AddBuff(BuffID.PotionSickness, 600);
        Player.ClearBuff(ModContent.BuffType<PiratePower>());
    }
}

public override void PostUpdateBuffs()
{
    if (!Player.HasBuff(ModContent.BuffType<PiratePower>()) && hasPiratePower && !wasHitDuringBuff)
    {
        int coins = CountGoldCoinsInInventory();
        if (coins > 10) coins = 10;
        Player.statLife += coins * 10;
        Player.HealEffect(coins * 10, true);

        for (int i = 0; i < coins; i++)
            Player.QuickSpawnItem(Player.GetSource_Misc("PiratePowerEnd"), ItemID.GoldCoin);

        wasHitDuringBuff = false;
    }
}

private int CountGoldCoinsInInventory()
{
    int totalGold = 0;
    foreach (Item item in Player.inventory)
    {
        if (item.type == ItemID.GoldCoin)
            totalGold += item.stack;
    }
    return totalGold;
}

    }
}
