namespace MightofUniverses.Common.Players
{
    public class CritOverflowPlayer : ModPlayer
    {
        private const float OverflowConversion = 1.5f;

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            ApplyCritOverflow(ref modifiers);
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            ApplyCritOverflow(ref modifiers);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            ApplyCritOverflow(ref modifiers);
        }

        private void ApplyCritOverflow(ref NPC.HitModifiers modifiers)
        {
            int totalCrit = Player.GetWeaponCrit(Player.HeldItem);

            if (totalCrit > 100)
            {
                float overflow = totalCrit - 100;
                float bonusCritDamage = overflow * (OverflowConversion / 100f);

                modifiers.CritDamage += bonusCritDamage;
            }
        }
    }
}