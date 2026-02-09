using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using Terraria.GameContent;

namespace MightofUniverses.Content.Items. Projectiles
{
    public class FrigidHeartShard :  MoUProjectile
    {
        private float orbitRadius = 50f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile. timeLeft = 999999;
            Projectile. ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (! owner.GetModPlayer<FrigidHeartPlayer>().hasFrigidHeart)
            {
                Projectile.Kill();
                return;
            }

            if (Projectile.ai[0] == 0f)
            {
                AI_Orbiting(owner);
            }
            else if (Projectile.ai[0] == 1f)
            {
                AI_Fired();
            }

            Lighting.AddLight(Projectile. Center, 0.3f, 0.5f, 0.8f);

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.velocity *= 0.2f;
            }

            Projectile.rotation += 0.08f;
        }

        private void AI_Orbiting(Player owner)
        {
            int orbitIndex = 0;
            int totalOrbitingShards = 0;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == Projectile.type && proj.owner == Projectile.owner && proj.ai[0] == 0f)
                {
                    if (proj.whoAmI == Projectile.whoAmI)
                    {
                        orbitIndex = totalOrbitingShards;
                    }
                    totalOrbitingShards++;
                }
            }

            if (totalOrbitingShards == 0)
                totalOrbitingShards = 1;

            float baseAngle = MathHelper.TwoPi / 10f * orbitIndex;
            float rotationSpeed = 0.02f;
            Projectile.ai[1] += rotationSpeed;

            float currentAngle = baseAngle + Projectile.ai[1];

            Vector2 offset = new Vector2(
                (float)Math.Cos(currentAngle) * orbitRadius,
                (float)Math.Sin(currentAngle) * orbitRadius
            );

            Projectile. Center = owner.Center + offset;
            Projectile.velocity = Vector2.Zero;
            Projectile.rotation = currentAngle;
            Projectile.tileCollide = false;
        }

        private void AI_Fired()
        {
            Projectile.tileCollide = true;

            if (Projectile.velocity.Length() > 0.1f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }

            NPC target = FindNearestEnemy();
            if (target != null)
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                float currentSpeed = Projectile.velocity.Length();
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * Math.Max(currentSpeed, 14f), 0.04f);
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, default, 1.2f);
                dust. noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        private NPC FindNearestEnemy()
        {
            NPC closest = null;
            float closestDist = 1000f;

            foreach (NPC npc in Main. npc)
            {
                if (npc.active && ! npc.friendly && npc.lifeMax > 5 && npc.type != NPCID. TargetDummy && ! npc.dontTakeDamage)
                {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = npc;
                    }
                }
            }

            return closest;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 180);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile. height, DustID.Ice, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 100, default, 1.5f);
                dust.noGravity = true;
            }

            if (Projectile.ai[0] == 1f)
            {
                Projectile.penetrate--;
                if (Projectile.penetrate <= 0)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] == 1f)
            {
                return true;
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 100, default, 1.3f);
                dust.noGravity = true;
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type]. Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(texture, drawPos, null, Color. White, Projectile.rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, new Color(100, 150, 255, 100), Projectile.rotation, texture. Size() / 2f, Projectile.scale * 1.2f, SpriteEffects. None, 0);

            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}