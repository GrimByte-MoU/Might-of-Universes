using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ReapersEchoUltimateMirage : MoUProjectile
    {
        private int playerFrame = 0;
        private int playerDirection = 1;
        private float fadeAlpha = 1f;
        private bool hasCharged = false;
        private const int DELAY_TIME = 30; // Hover for 0.5 seconds

        public override string Texture => "Terraria/Images/Player_0";

        public override void SafeSetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.scale = 1.3f;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            // Store player info on spawn
            if (Projectile.localAI[0] == 0f)
            {
                playerFrame = owner.bodyFrame.Y / owner.bodyFrame.Height;
                playerDirection = Main.MouseWorld.X > Projectile.Center.X ? 1 : -1;
                Projectile.localAI[0] = 1f;
            }

            Projectile.ai[0]++;

            // Phase 1: Hover in place
            if (Projectile.ai[0] < DELAY_TIME && !hasCharged)
            {
                Projectile.velocity = Vector2.Zero;
                
                // Pulsing effect
                Projectile.scale = 1.3f + (float)Math.Sin(Projectile.ai[0] * 0.2f) * 0.1f;

                // Charging dust
                if (Main.rand.NextBool(2))
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                        DustID.Shadowflame, 0f, 0f, 100, Color.Purple, 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.2f;
                }
            }
            // Phase 2: Charge at cursor
            else if (!hasCharged)
            {
                hasCharged = true;
                Vector2 toCursor = Main.MouseWorld - Projectile.Center;
                toCursor.Normalize();
                Projectile.velocity = toCursor * 25f;

                // Charge sound
                SoundEngine.PlaySound(SoundID.Item8, Projectile.position);

                // Burst of dust
                for (int i = 0; i < 20; i++)
                {
                    Vector2 dustVel = Main.rand.NextVector2Circular(5f, 5f);
                    int dust = Dust.NewDust(Projectile.Center, 4, 4, DustID.Shadowflame,
                        dustVel.X, dustVel.Y, 100, Color.Purple, 2f);
                    Main.dust[dust].noGravity = true;
                }
            }

            // Fade out at end
            if (Projectile.timeLeft < 30)
            {
                fadeAlpha = Projectile.timeLeft / 30f;
            }

            // Trail during charge
            if (hasCharged && Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Shadowflame, 0f, 0f, 100, Color.Purple, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Massive impact effect
            SoundEngine.PlaySound(SoundID.NPCHit53, target.Center);
            
            for (int i = 0; i < 25; i++)
            {
                Vector2 dustVel = Main.rand.NextVector2Circular(8f, 8f);
                int dust = Dust.NewDust(target.Center, 4, 4, DustID.Shadowflame,
                    dustVel.X, dustVel.Y, 100, Color.Purple, 2.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Player owner = Main.player[Projectile.owner];
            
            // Get player texture - TWO INDICES REQUIRED
            Texture2D playerTexture = TextureAssets.Players[owner.skinVariant, 0].Value;
            
            int frameHeight = playerTexture.Height / 20;
            Rectangle sourceRect = new Rectangle(0, playerFrame * frameHeight, playerTexture.Width, frameHeight);
            
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            
            // Glowing effect for ultimate mirages
            Color glowColor = Color.Purple * fadeAlpha * 0.5f;
            Main.EntitySpriteDraw(playerTexture, drawPos, sourceRect, glowColor, Projectile.rotation,
                sourceRect.Size() * 0.5f, Projectile.scale * 1.2f, SpriteEffects.None, 0);

            // Main draw
            Color drawColor = Color.White * fadeAlpha * 0.8f;
            SpriteEffects effects = playerDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            
            Main.EntitySpriteDraw(playerTexture, drawPos, sourceRect, drawColor, Projectile.rotation,
                sourceRect.Size() * 0.5f, Projectile.scale, effects, 0);

            return false;
        }
    }
}