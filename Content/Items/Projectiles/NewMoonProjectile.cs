using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class NewMoonProjectile : MoUProjectile
    {
        private int stationaryTime = 0;
        private const int STATIONARY_DURATION = 15; // ~0.25 seconds
        private bool hasAccelerated = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.alpha = 0;
            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            // Phase 1: Stationary
            if (stationaryTime < STATIONARY_DURATION)
            {
                stationaryTime++;
                if (Projectile.ai[0] == 0f)
                {
                    // Store velocity direction in ai slots
                    Projectile.ai[0] = Projectile.velocity.X;
                    Projectile.ai[1] = Projectile.velocity.Y;
                    Projectile.velocity = Vector2.Zero;
                }
                Projectile.scale = 1f + (float)System.Math.Sin(stationaryTime * 0.3f) * 0.15f;
                if (Main.rand.NextBool(3))
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                        DustID.LunarOre, 0f, 0f, 100, default, 1.2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.2f;
                }
            }
            // Phase 2: Rapid acceleration
            else if (!hasAccelerated)
            {
                hasAccelerated = true;
                Vector2 direction = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                direction.Normalize();
                Projectile.velocity = direction * 3f; // Start slow
                SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
                for (int i = 0; i < 15; i++)
                {
                    Vector2 dustVel = Main.rand.NextVector2Circular(4f, 4f);
                    int dust = Dust.NewDust(Projectile.Center, 4, 4, DustID.Shadowflame, 
                        dustVel.X, dustVel.Y, 100, default, 1.5f);
                    Main.dust[dust].noGravity = true;
                }

                Projectile.scale = 1f;
            }
            else
            {
                if (Projectile.velocity.Length() < 22f)
                {
                    Projectile.velocity *= 1.08f;
                }
                Projectile.rotation += 0.3f;
                if (Main.rand.NextBool(2))
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                        DustID.LunarOre, 0f, 0f, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.5f;
                }
            }
            Lighting.AddLight(Projectile.Center, 0.1f, 0.1f, 0.15f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Dark impact effect
            for (int i = 0; i < 8; i++)
            {
                Vector2 dustVel = Main.rand.NextVector2Circular(3f, 3f);
                int dust = Dust.NewDust(target.Center, 4, 4, DustID.Shadowflame,
                    dustVel.X, dustVel.Y, 100, default, 1.3f);
                Main.dust[dust].noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.NPCHit53, target.Center);
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(7f, target.Center);
            target.AddBuff(ModContent.BuffType<LunarReap>(), 120);
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;

            // Draw dark trail
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Color trailColor = new Color(20, 20, 40) * alpha * 0.6f;
                Main.EntitySpriteDraw(texture, drawPos, null, trailColor, Projectile.rotation, 
                    drawOrigin, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            }

            // Dark glow if stationary
            if (stationaryTime < STATIONARY_DURATION)
            {
                Color glowColor = new Color(40, 40, 80, 0) * 0.5f;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, 
                    glowColor, Projectile.rotation, drawOrigin, Projectile.scale * 1.3f, SpriteEffects.None, 0);
            }

            // Main projectile (darkened)
            Color mainColor = Color.Lerp(lightColor, new Color(30, 30, 50), 0.5f);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, 
                mainColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            // New moon = dark appearance
            return new Color(30, 30, 60, 200);
        }
    }
}