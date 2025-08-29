using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class DamageHealingPlayer : ModPlayer
    {
        public bool hasHealOnHitAccessory;

        public override void ResetEffects()
        {
            hasHealOnHitAccessory = false;
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (hasHealOnHitAccessory && Main.rand.NextFloat() <= 0.05f)
            {
                int healAmount = (int)(info.Damage * 0.5);
                Player.statLife += healAmount;
                Player.HealEffect(healAmount);
            }
        }
    }
}
