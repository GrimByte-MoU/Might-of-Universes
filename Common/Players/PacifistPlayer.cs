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

    // For items used directly as weapons
    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        if (item.DamageType == ModContent.GetInstance<PacifistDamageClass>())
        {
            damage *= pacifistDamageMultiplier;
        }
    }

    // For projectiles spawned by accessories/armor/etc.
    public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (proj.DamageType == ModContent.GetInstance<PacifistDamageClass>())
        {
            modifiers.FinalDamage *= pacifistDamageMultiplier;
        }
    }
}

}
