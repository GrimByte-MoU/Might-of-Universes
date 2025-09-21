using Microsoft.Xna.Framework;
using Terraria;
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
            float tiles = BaseAuraRadiusTiles + BonusAuraRadiusTiles;
            if (tiles < 0f) tiles = 0f; // safety
            return tiles * 16f;
        }

        public override void PostUpdate()
        {
            if (!HasGuidingLightSource || BaseAuraRadiusTiles <= 0f)
                return;

            float radiusPx = GetAuraRadiusPixels();
            int buffType = ModContent.BuffType<GuidingLight>();
            const int applyTime = 120;

            // Self always gets it
            Player.AddBuff(buffType, applyTime);

            // Grant to nearby allies
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player other = Main.player[i];
                if (other == null || !other.active || other.dead || other.whoAmI == Player.whoAmI)
                    continue;
                if (Vector2.Distance(other.Center, Player.Center) <= radiusPx)
                {
                    other.AddBuff(buffType, applyTime);
                }
            }
        }
    }
}