namespace MightofUniverses.Common.Players
{
    public class PartyPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            bool hasFireMage = Player.HasBuff(ModContent.BuffType<FireMageBuff>());
            bool hasFrostWarrior = Player.HasBuff(ModContent.BuffType<FrostWarriorBuff>());
            bool hasWindRanger = Player.HasBuff(ModContent.BuffType<WindRangerBuff>());
            bool hasNatureHealer = Player.HasBuff(ModContent.BuffType<NatureHealerBuff>());

            if (hasFireMage && hasFrostWarrior && hasWindRanger && hasNatureHealer)
                Player.AddBuff(ModContent.BuffType<FullPartyBuff>(), 2);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Player.HasBuff(ModContent.BuffType<FullPartyBuff>()))
                return;

            if (!proj.minion && !proj.sentry)
                return;

            int healAmount = (int)(damageDone * 0.005f);

            if (damageDone > 0 && healAmount < 1)
                healAmount = 1;

            Player.statLife = System.Math.Min(Player.statLife + healAmount, Player.statLifeMax2);
            Player.HealEffect(healAmount);
        }
    }
}