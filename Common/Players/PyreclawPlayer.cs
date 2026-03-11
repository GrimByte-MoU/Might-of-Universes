using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Weapons;

namespace MightofUniverses.Content.Items
{
    public class PyreclawPlayer : ModPlayer
    {
        public override void PostHurt(Player.HurtInfo info)
        {
            if (Player.HeldItem.type == ModContent.ItemType<Pyreclaw>())
            {
                int damageThreshold = (int)(Player.statLifeMax2 * 0.1f);

                if (info.Damage >= damageThreshold)
                {
                    FireRetaliationBurst();
                }
            }
        }

        private void FireRetaliationBurst()
        {
            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact with { Volume = 0.6f }, Player.Center);
            SoundEngine.PlaySound(SoundID.Item73, Player.Center);

            int damage = Player.GetWeaponDamage(Player.HeldItem);

            for (int i = 0; i < 8; i++)
            {
                float angle = MathHelper.TwoPi / 8f * i;
                Vector2 velocity = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * 14f;

                Projectile.NewProjectile(
                    Player.GetSource_OnHurt(null),
                    Player.Center,
                    velocity,
                    ModContent.ProjectileType<PyreclawFireball>(),
                    (int)(damage * 0.75f),
                    4f,
                    Player.whoAmI
                );
            }

            for (int d = 0; d < 40; d++)
            {
                Vector2 dustVel = Main.rand.NextVector2Circular(8f, 8f);
                Dust dust = Dust.NewDustDirect(Player.Center, 10, 10, DustID.Torch, dustVel.X, dustVel.Y, 100, Color.OrangeRed, 2.5f);
                dust.noGravity = true;
            }
        }
    }
}