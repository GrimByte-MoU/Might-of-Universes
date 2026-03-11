using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SharpLeaf : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.extraUpdates = 5;
            Projectile.alpha = 0;
            Projectile.usesLocalNPCImmunity = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.JungleGrass,
                    0f, 0f,
                    100,
                    default,
                    1.0f
                );
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            if (Projectile.timeLeft % 10 == 0)
            {
                Dust trail = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.JunglePlants,
                    -Projectile.velocity * 0.2f,
                    100,
                    Color.LimeGreen,
                    1.3f
                );
                trail.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.BetsysCurse, 300);
            target.AddBuff(ModContent.BuffType<NaturesToxin>(), 180);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.JunglePlants,
                    0f, 0f,
                    100,
                    Color.Green,
                    1.5f
                );
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                dust.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.JungleGrass
                );
                dust.velocity = Main.rand.NextVector2Circular(2f, 2f);
            }

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.JunglePlants,
                    0f, 0f,
                    100,
                    default,
                    1.2f
                );
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                dust.noGravity = true;
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Color glowColor = Color.Lerp(Color.LimeGreen, Color.White, 0.5f) * (1f - Projectile.alpha / 255f);

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                glowColor * 0.7f,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale * 1.1f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor * (1f - Projectile.alpha / 255f),
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}