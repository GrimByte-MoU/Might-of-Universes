using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class IceAgeSpike : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 2.0f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
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

                Main.EntitySpriteDraw(texture, trailPos, null, trailColor, Projectile.oldRot[i], drawOrigin, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, Color.White * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SheerCold>(), 120);

            Player player = Main.player[Projectile.owner];
            if (player.active && !player.dead)
            {
                ReaperPlayer reaperPlayer = player.GetModPlayer<ReaperPlayer>();
                reaperPlayer.AddSoulEnergy(6f, target.Center);
            }

            ShatterIntoFragments(target.Center);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 2.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            }
        }

        private void ShatterIntoFragments(Vector2 hitPosition)
        {
            SoundEngine.PlaySound(SoundID.Item107, hitPosition);

            for (int i = 0; i < 5; i++)
            {
                float angle = MathHelper.TwoPi * i / 5f;
                Vector2 velocity = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * 12f;

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    hitPosition,
                    velocity,
                    ModContent.ProjectileType<IceAgeFragment>(),
                    (int)(Projectile.damage * 0.5f),
                    Projectile.knockBack * 0.5f,
                    Projectile.owner
                );
            }

            for (int i = 0; i < 30; i++)
            {
                Dust shatter = Dust.NewDustDirect(hitPosition - Vector2.One * 20, 40, 40, DustID.SnowflakeIce, 0f, 0f, 100, Color.White, 3.0f);
                shatter.noGravity = true;
                shatter.velocity = Main.rand.NextVector2Circular(8f, 8f);
            }
        }
    }
}