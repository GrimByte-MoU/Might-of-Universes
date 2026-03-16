using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class BaseCyberModulePlayer : ModPlayer
    {
        public bool hasBaseCyberModule = false;
        private int ringFireTimer = 0;

        public override void ResetEffects()
        {
            hasBaseCyberModule = false;
        }

        public override void PostUpdate()
        {
            if (!hasBaseCyberModule) return;

            bool hasCyberOneSet = Player.GetModPlayer<CyberOnePlayer>().hasCyberOneSet;
            int fireInterval = hasCyberOneSet ? 30 : 60;

            ringFireTimer++;
            if (ringFireTimer >= fireInterval)
            {
                ringFireTimer = 0;
                FireCyberRing(hasCyberOneSet);
            }
        }

        private void FireCyberRing(bool hasCyberOneSet)
        {
            if (Main.myPlayer != Player.whoAmI) return;

            int damage = hasCyberOneSet ? 200 : 150;
            
            Vector2 velocity = new Vector2(3f, 0).RotatedByRandom(MathHelper.TwoPi);

            int projIndex = Projectile.NewProjectile(
                Player.GetSource_Accessory(Player.armor[0]),
                Player.Center,
                velocity,
                ModContent.ProjectileType<CyberRing>(),
                damage,
                2f,
                Player.whoAmI,
                hasCyberOneSet ? 1f : 0f
            );
        }
    }
}