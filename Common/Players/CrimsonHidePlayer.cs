using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Common.Players
{
    public class CrimsonHidePlayer : ModPlayer
    {
        public bool hasCrimsonHideSet = false;

        public override void ResetEffects()
        {
            hasCrimsonHideSet = false;
        }

        public override void PostUpdateEquips()
        {
            if (!hasCrimsonHideSet) return;

            // Apply set bonus stats
            Player.GetDamage(DamageClass.Generic) *= 0.80f; // -20% weapon damage (set bonus)
            Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.30f; // +30% pacifist damage (set bonus)
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasCrimsonHideSet) return;
            if (proj.DamageType != ModContent.GetInstance<Common.PacifistDamageClass>()) return;

            ApplyLifesteal(damageDone);
        }

        private void ApplyLifesteal(int damageDone)
        {
            // 50% chance to lifesteal
            if (Main.rand.NextBool(2))
            {
                int healAmount;

                if (Main.rand.NextBool(2))
                {
                    healAmount = (int)(damageDone * 0.02f); // 0.5% + 1.5% = 2%
                }
                else
                {
                    healAmount = (int)(damageDone * 0.005f); // 0.5%
                }

                // Minimum 1 HP if damage was dealt
                if (damageDone > 0 && healAmount < 1)
                    healAmount = 1;

                if (healAmount > 0)
                {
                    Player.statLife += healAmount;
                    if (Player.statLife > Player.statLifeMax2)
                        Player.statLife = Player.statLifeMax2;

                    Player.HealEffect(healAmount);

                    for (int i = 0; i < 3; i++)
                    {
                        Dust dust = Dust.NewDustDirect(
                            Player.position,
                            Player.width,
                            Player.height,
                            DustID.Blood,
                            Main.rand.NextFloat(-1f, 1f),
                            Main.rand.NextFloat(-1f, 1f),
                            0,
                            default,
                            Main.rand.NextFloat(0.8f, 1.2f)
                        );
                        dust.noGravity = true;
                    }
                }
            }
        }
    }
}