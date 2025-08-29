using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace MightofUniverses.Content.Items.Buffs;
public class LunarVision : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = false;
        Main.buffNoSave[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.GetCritChance(DamageClass.Generic) += 25f;
        player.GetAttackSpeed(DamageClass.Generic) += 0.25f;
        player.moveSpeed += 0.75f;
    }
}
