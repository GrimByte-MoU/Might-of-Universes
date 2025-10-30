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

            Player.GetDamage(DamageClass.Generic) *= 0.80f;
            Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.30f;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasCrimsonHideSet) return;
            if (proj.DamageType != ModContent.GetInstance<Common.PacifistDamageClass>()) return;

            ApplyLifesteal(damageDone);
        }

        private void ApplyLifesteal(int damageDone)
        {
            if (Main.rand.NextBool(2))
            {
                int healAmount;

                if (Main.rand.NextBool(2))
                {
                    healAmount = (int)(damageDone * 0.02f);
                }
                else
                {
                    healAmount = (int)(damageDone * 0.005f);
                }

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

        public override void PostUpdate()
        {
            if (!hasCrimsonHideSet) return;

            Lighting.AddLight(Player.Center, 0.9f, 0.1f, 0.1f);

            if (Main.rand.NextBool(3))
            {
                int dustType = Main.rand.NextBool(4) ? DustID.Blood : DustID.IchorTorch;
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, dustType, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Torch, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 100, Color.Red, 0.6f);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
            }
        }
    }
}