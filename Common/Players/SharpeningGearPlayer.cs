using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.Players
{
public class SharpeningGearPlayer : ModPlayer
{
    public bool hasSharpeningGear;

    public override void ResetEffects()
    {
        hasSharpeningGear = false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (hasSharpeningGear && Player.GetModPlayer<ReaperPlayer>().hasReaperArmor)
        {
            target.AddBuff(ModContent.BuffType<Shred>(), 180);
        }
    }
}
}