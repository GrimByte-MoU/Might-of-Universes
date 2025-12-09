using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses. Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    /// <summary>
    /// Handles Soul Lantern aura effects - applies Guiding Light buff to player and nearby allies. 
    /// </summary>
    public class LanternAuraPlayer : ModPlayer
    {
        /// <summary>Base aura radius in tiles (set by equipped lantern)</summary>
        public float BaseAuraRadiusTiles;

        /// <summary>Bonus aura radius from accessories/buffs</summary>
        public float BonusAuraRadiusTiles;

        /// <summary>Whether player is currently holding a lantern that grants Guiding Light</summary>
        public bool HasGuidingLightSource;

        public override void ResetEffects()
        {
            BaseAuraRadiusTiles = 0f;
            BonusAuraRadiusTiles = 0f;
            HasGuidingLightSource = false;
        }

        /// <summary>Calculates total aura radius in pixels</summary>
        public float GetAuraRadiusPixels()
        {
            float totalTiles = BaseAuraRadiusTiles + BonusAuraRadiusTiles;
            if (totalTiles < 0f) totalTiles = 0f;
            return totalTiles * 16f; // 1 tile = 16 pixels
        }

        public override void PostUpdate()
        {
            if (Player.dead || !HasGuidingLightSource || BaseAuraRadiusTiles <= 0f)
                return;

            float radiusPixels = GetAuraRadiusPixels();
            int buffType = ModContent.BuffType<GuidingLight>();
            const int BuffDuration = 10;

            Player.AddBuff(buffType, BuffDuration);

            if (Main.netMode != 0)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player ally = Main.player[i];
                    if (ally == null || ! ally.active || ally.dead || ally.whoAmI == Player.whoAmI)
                        continue;
                    if (Vector2. Distance(ally.Center, Player.Center) <= radiusPixels)
                    {
                        ally.AddBuff(buffType, BuffDuration);
                    }
                }
            }
        }
    }
}