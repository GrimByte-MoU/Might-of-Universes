using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FrozenTyrantSphere : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 2.0f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.fadeIn = 1.5f;
            }

            if (Main.rand.NextBool(3))
            {
                Dust frost = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SnowflakeIce, 0f, 0f, 100, Color.White, 1.5f);
                frost.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.7f, 1.0f);
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
                Color trailColor = Color.Cyan * trailAlpha * 0.5f;

                Main.EntitySpriteDraw(
                    texture,
                    trailPos,
                    null,
                    trailColor,
                    Projectile.oldRot[i],
                    drawOrigin,
                    Projectile.scale * (1f - i * 0.08f),
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
                Color.White * 0.5f,
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
            target.AddBuff(ModContent.BuffType<SheerCold>(), 180);

            if (!target.boss && Main.rand.NextFloat() < 0.2f)
            {
                target.AddBuff(ModContent.BuffType<Paralyze>(), 120);
            }

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 2.0f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 2.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
            }
        }
    }
}