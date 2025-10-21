using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Weapons;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class RainbowsEdgeProjectile : MoUProjectile
    {
        protected virtual float HoldoutRangeMin => 44f;
        protected virtual float HoldoutRangeMax => 116f;

        public override void SafeSetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear);
            Projectile.scale = 2f;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            int duration = player.itemAnimationMax;

            player.heldProj = Projectile.whoAmI;

            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }

            Projectile.velocity = Vector2.Normalize(Projectile.velocity);

            float halfDuration = duration * 0.5f;
            float progress;

            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }

            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                Projectile.rotation += MathHelper.ToRadians(135f);
            }

            if (Projectile.timeLeft == duration - 1)
            {
                Vector2 velocity = Vector2.Normalize(Projectile.velocity) * 12f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Vector2.Normalize(Projectile.velocity) * 10f, velocity, 
                    ModContent.ProjectileType<PrismaticSpearhead>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
        }
    }
}
