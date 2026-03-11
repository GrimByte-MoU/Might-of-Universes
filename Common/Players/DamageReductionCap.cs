using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class DamageReductionCap : ModPlayer
    {
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            float totalReduction = 1f - modifiers.FinalDamage.Multiplicative;

            if (totalReduction > 0.75f)
            {
                modifiers.FinalDamage *= 1f / (1f - 0.75f);
                modifiers.FinalDamage *= 1f - totalReduction;
            }
        }
    }
}