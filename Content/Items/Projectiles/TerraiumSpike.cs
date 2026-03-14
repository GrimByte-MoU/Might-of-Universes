using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TerraiumSpike : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            int targetIndex = (int)Projectile.ai[0];
            int delay = (int)Projectile.ai[1];

            if (Projectile.localAI[0] < delay)
            {
                Projectile.localAI[0]++;
                Projectile.velocity = Vector2.Zero;
                return;
            }

            if (Projectile.localAI[1] == 0)
            {
                Projectile.localAI[1] = 1;

                if (targetIndex >= 0 && targetIndex < Main.maxNPCs && Main.npc[targetIndex].active)
                {
                    NPC target = Main.npc[targetIndex];
                    Vector2 directionToTarget = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY);
                    Projectile.velocity = directionToTarget * 20f;
                }
                else
                {
                    Projectile.velocity = new Vector2(0, 20f);
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 1.0f, 0.3f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 60);
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Rectangle sourceRect = texture.Frame(1, 1, 0, 0);
            Vector2 origin = sourceRect.Size() / 2f;

            Main.EntitySpriteDraw(
                texture,
                drawPosition,
                sourceRect,
                lightColor * (1f - Projectile.alpha / 255f),
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, default, 1.8f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
            }
        }
    }
}