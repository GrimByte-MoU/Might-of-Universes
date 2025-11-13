using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ReapersEchoMirage : MoUProjectile
    {
        private int playerFrame = 0;
        private int playerDirection = 1;
        private float fadeAlpha = 1f;

        public override string Texture => "Terraria/Images/Player_0"; // Use player texture

        public override void SafeSetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            // Store player info on spawn
            if (Projectile.localAI[0] == 0f)
            {
                playerFrame = owner.bodyFrame.Y / owner.bodyFrame.Height;
                playerDirection = owner.direction;
                Projectile.localAI[0] = 1f;
            }

            // Fade out over time
            fadeAlpha = Projectile.timeLeft / 60f;

            // Slow down over time
            Projectile.velocity *= 0.97f;

            // Dust trail
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Shadowflame, 0f, 0f, 100, Color.Purple, 1.2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player owner = Main.player[Projectile.owner];
            ReaperPlayer reaperPlayer = owner.GetModPlayer<ReaperPlayer>();

            // Grant 8 souls per hit
            reaperPlayer.AddSoulEnergy(8f, target.Center);

            // Impact effect
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(target.Center, 4, 4, DustID.Shadowflame, 0f, 0f, 100, Color.Purple, 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Player owner = Main.player[Projectile.owner];
            Texture2D playerTexture = TextureAssets.Players[owner.skinVariant, 0].Value;
            
            // Calculate the source rectangle (player body frame)
            int frameHeight = playerTexture.Height / 20; // Players have 20 animation frames
            Rectangle sourceRect = new Rectangle(0, playerFrame * frameHeight, playerTexture.Width, frameHeight);
            
            // Draw position
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            
            // Draw with fade
            Color drawColor = Color.White * fadeAlpha * 0.7f;
            SpriteEffects effects = playerDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            
            Main.EntitySpriteDraw(playerTexture, drawPos, sourceRect, drawColor, Projectile.rotation,
                sourceRect.Size() * 0.5f, 1f, effects, 0);

            return false;
        }
    }
}