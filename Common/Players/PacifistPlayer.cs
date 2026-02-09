using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class PacifistPlayer : ModPlayer
{
    public float pacifistDamageMultiplier = 1f;

    public override void ResetEffects()
    {
        pacifistDamageMultiplier = 1f;
    }
    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        if (item.DamageType == ModContent.GetInstance<PacifistDamageClass>())
        {
            damage *= pacifistDamageMultiplier;
        }
    }
    public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (proj.DamageType == ModContent.GetInstance<PacifistDamageClass>())
        {
            modifiers.FinalDamage *= pacifistDamageMultiplier;
        }
    }
}

}
