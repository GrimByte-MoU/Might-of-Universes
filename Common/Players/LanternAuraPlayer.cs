using Microsoft. Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class LanternAuraPlayer : ModPlayer
    {
        public float BaseAuraRadiusTiles;
        public float BonusAuraRadiusTiles;
        public bool HasGuidingLightSource;

        public override void ResetEffects()
        {
            BaseAuraRadiusTiles = 0f;
            BonusAuraRadiusTiles = 0f;
            HasGuidingLightSource = false;
        }

        public float GetAuraRadiusPixels()
        {
            float totalTiles = BaseAuraRadiusTiles + BonusAuraRadiusTiles;
            if (totalTiles < 0f) totalTiles = 0f;
            return totalTiles * 16f;
        }

        public override void PostUpdate()
        {
            if (Player.dead || !HasGuidingLightSource || BaseAuraRadiusTiles <= 0f)
                return;

            float radiusPixels = GetAuraRadiusPixels();
            int buffType = ModContent.BuffType<GuidingLight>();
            const int BuffDuration = 10;

            Player.AddBuff(buffType, BuffDuration);

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player ally = Main.player[i];

                    if (ally == null || ! ally.active || ally.dead || ally.whoAmI == Player.whoAmI)
                        continue;

                    if (Vector2.Distance(ally.Center, Player.Center) <= radiusPixels)
                    {
                        ally.AddBuff(buffType, BuffDuration);
                    }
                }
            }

            DrawAuraCircle();
        }

        private void DrawAuraCircle()
        {
            if (!HasGuidingLightSource || BaseAuraRadiusTiles <= 0f)
                return;

            float radius = GetAuraRadiusPixels();
            int segments = 64;
            float pulseAlpha = 0.4f + (float)System.Math.Sin(Main.GameUpdateCount * 0.08f) * 0.2f;

            for (int i = 0; i < segments; i++)
            {
                if (Main.rand.NextBool(2))
                    continue;

                float angle = MathHelper.TwoPi / segments * i;
                Vector2 offset = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * radius;
                Vector2 dustPos = Player.Center + offset;

                Dust dust = Dust. NewDustPerfect(dustPos, DustID.YellowTorch, Vector2.Zero, 100, 
                    new Color(230, 200, 100, 0) * pulseAlpha, 1.2f);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.fadeIn = 1f;
            }
        }
    }
}