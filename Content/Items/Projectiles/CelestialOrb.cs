using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CelestialOrb : MoUProjectile
    {
        private int homingTimer = 0;
        private int colorType = 0;

        public override void SafeSetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 50;
            Projectile.scale = 0.5f;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                colorType = Main.rand.Next(4);
                Projectile.ai[0] = 1f;
            }

            homingTimer++;
            if (homingTimer > 10)
            {
                NPC target = FindClosestNPC(Projectile.Center, 400f);
                if (target != null)
                {
                    Vector2 desiredVelocity = target.Center - Projectile.Center;
                    desiredVelocity.Normalize();
                    desiredVelocity *= 12f;

                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.05f);
                }
            }

            Projectile.rotation += 0.2f;
            if (Main.rand.NextBool(3))
            {
                int dustType = colorType switch
                {
                    0 => DustID.SolarFlare,
                    1 => DustID.Vortex,
                    2 => DustID.Clentaminator_Cyan,
                    3 => DustID.PurpleTorch,
                    _ => DustID.RainbowMk2
                };

                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 0.8f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            ReaperPlayer reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.AddSoulEnergy(1f, target.Center);
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 100, default, 1.2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 2f;
            }
            target.AddBuff(BuffID.Daybreak, 60);
            target.AddBuff(BuffID.Frostburn2, 60);
            target.AddBuff(BuffID.Venom, 60);
            target.AddBuff(BuffID.Ichor, 60);
        }

        private NPC FindClosestNPC(Vector2 position, float maxDistance)
        {
            NPC closest = null;
            float closestDist = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && npc.CanBeChasedBy())
                {
                    float dist = Vector2.Distance(position, npc.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = npc;
                    }
                }
            }

            return closest;
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color orbColor = colorType switch
            {
                0 => new Color(255, 200, 100),
                1 => new Color(100, 255, 200),
                2 => new Color(100, 200, 255),
                3 => new Color(255, 100, 255),
                _ => Color.White
            };
            Color glowColor = orbColor * 0.5f;
            glowColor.A = 0;
            Main.EntitySpriteDraw(texture, drawPos, null, glowColor, Projectile.rotation, drawOrigin, Projectile.scale * 1.3f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, orbColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return colorType switch
            {
                0 => new Color(255, 200, 100, 200),
                1 => new Color(100, 255, 200, 200),
                2 => new Color(100, 200, 255, 200),
                3 => new Color(255, 100, 255, 200),
                _ => Color.White
            };
        }
    }
}