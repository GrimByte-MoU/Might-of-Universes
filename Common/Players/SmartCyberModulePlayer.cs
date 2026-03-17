using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class SmartCyberModulePlayer : ModPlayer
    {
        public bool hasSmartCyberModule = false;
        private int sawFireTimer = 0;

        public override void ResetEffects()
        {
            hasSmartCyberModule = false;
        }

        public override void PostUpdate()
        {
            if (!hasSmartCyberModule) return;

            bool hasCyberOneSet = Player.GetModPlayer<CyberOnePlayer>().hasCyberOneSet;

            sawFireTimer++;
            if (sawFireTimer >= 60)
            {
                sawFireTimer = 0;
                FireCyberSaw(hasCyberOneSet);
            }
        }

        private void FireCyberSaw(bool hasCyberOneSet)
        {
            if (Main.myPlayer != Player.whoAmI) return;

            int damage = hasCyberOneSet ? 400 : 200;
            
            Vector2 velocity = new Vector2(4f, 0).RotatedByRandom(MathHelper.TwoPi);

            Projectile.NewProjectile(
                Player.GetSource_Accessory(Player.armor[0]),
                Player.Center,
                velocity,
                ModContent.ProjectileType<CyberSaw>(),
                damage,
                3f,
                Player.whoAmI
            );
        }
    }
}