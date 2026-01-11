using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Weapons;
using MightofUniverses.Content. Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class TerraiumBowPlayer : ModPlayer
    {
        private int harpoonCooldown = 0;

        public override void PostUpdate()
        {
            if (harpoonCooldown > 0)
            {
                harpoonCooldown--;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Player.HeldItem.type == ModContent.ItemType<TerraiumBow>() && hit. Crit && harpoonCooldown <= 0)
            {
                if (proj.arrow)
                {
                    Vector2 direction = (target.Center - Player.Center).SafeNormalize(Vector2.UnitX);
                    int harpoonDamage = (int)(Player.HeldItem.damage * 5.0f);

                    Projectile.NewProjectile(
                        Player.GetSource_ItemUse(Player.HeldItem),
                        Player.Center,
                        direction * 24f,
                        ModContent.ProjectileType<AncientHarpoon>(),
                        harpoonDamage,
                        5f,
                        Player.whoAmI
                    );

                    harpoonCooldown = 30;
                }
            }
        }
    }
}