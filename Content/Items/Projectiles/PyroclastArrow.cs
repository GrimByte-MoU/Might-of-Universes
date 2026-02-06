using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PyroclastArrow : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.arrow = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 15f)
            {
                Projectile.velocity.Y += 0.25f;
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, Color.OrangeRed, 2.0f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.fadeIn = 1.2f;
            }

            if (Main.rand.NextBool(3))
            {
                Dust ember = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FlameBurst, 0f, 0f, 100, Color.Red, 1.5f);
                ember.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 1.0f, 0.5f, 0.2f);
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float trailAlpha = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Color trailColor = Color.OrangeRed * trailAlpha * 0.5f;

                Main.EntitySpriteDraw(
                    texture,
                    trailPos,
                    null,
                    trailColor,
                    Projectile.oldRot[i],
                    drawOrigin,
                    Projectile.scale * (1f - i * 0.1f),
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                Color.White * 0.6f,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                target.Center,
                Vector2.Zero,
                ModContent.ProjectileType<PyroclastMagma>(),
                (int)(Projectile.damage * 0.5f),
                0f,
                Projectile.owner,
                target.whoAmI
            );

            SoundEngine.PlaySound(SoundID.Item74, target.Center);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Torch, 0f, 0f, 100, Color.OrangeRed, 2.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            }

            for (int i = 0; i < 10; i++)
            {
                Dust lava = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Lava, 0f, 0f, 100, default, 2.0f);
                lava.noGravity = true;
                lava.velocity = Main.rand.NextVector2Circular(4f, 4f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, Color.Orange, 1.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
            }
        }
    }
}