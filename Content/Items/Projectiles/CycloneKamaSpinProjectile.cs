using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Buffs;
using System;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CycloneKamaSpinProjectile : MoUProjectile
    {
        private const float SpinRadius = 100f;
        private const float SpinSpeed = 0.3f;
        private float currentAngle = 0f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 54;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead || !player.channel)
            {
                Projectile.Kill();
                return;
            }

            player.itemTime = 2;
            player.itemAnimation = 2;
            player.heldProj = Projectile.whoAmI;

            currentAngle += SpinSpeed;
            if (currentAngle >= MathHelper.TwoPi)
            {
                currentAngle -= MathHelper.TwoPi;
            }

            Vector2 offset = new Vector2(
                (float)Math.Cos(currentAngle) * SpinRadius,
                (float)Math.Sin(currentAngle) * SpinRadius
            );

            Projectile.Center = player.MountedCenter + offset;
            Projectile.rotation = currentAngle + MathHelper.PiOver4; // 45 degree tilt

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 2.0f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.7f, 1.0f);
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D chainTexture = ModContent.Request<Texture2D>("Terraria/Images/Misc/Perlin").Value;

            Vector2 playerCenter = player.MountedCenter - Main.screenPosition;
            Vector2 projectileCenter = Projectile.Center - Main.screenPosition;

            DrawChain(chainTexture, playerCenter, projectileCenter, Color.Cyan);

            Vector2 drawOrigin = texture.Size() * 0.5f;
            Main.EntitySpriteDraw(
                texture,
                projectileCenter,
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
                projectileCenter,
                null,
                Color.Cyan * 0.5f,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        private void DrawChain(Texture2D chainTexture, Vector2 start, Vector2 end, Color color)
        {
            Vector2 direction = end - start;
            float distance = direction.Length();
            direction.Normalize();

            int chainSegments = (int)(distance / 8f);
            for (int i = 0; i < chainSegments; i++)
            {
                Vector2 chainPos = start + direction * (i * 8f);
                Main.EntitySpriteDraw(
                    chainTexture,
                    chainPos,
                    new Rectangle(0, 0, 2, 2),
                    color * 0.7f,
                    direction.ToRotation() + MathHelper.PiOver2,
                    new Vector2(1, 1),
                    2f,
                    SpriteEffects.None,
                    0
                );
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(8f, target.Center);

            target.AddBuff(ModContent.BuffType<DeltaShock>(), 180);

            ChainLightning(target, damageDone);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Electric, 0f, 0f, 100, Color.Yellow, 2.0f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
            }
        }

        private void ChainLightning(NPC hitTarget, int baseDamage)
        {
            List<int> hitNPCs = new List<int> { hitTarget.whoAmI };
            NPC currentTarget = hitTarget;
            int chainDamage = (int)(baseDamage * 0.75f);

            for (int chain = 0; chain < 2; chain++)
            {
                NPC nextTarget = FindNearestEnemy(currentTarget.Center, 300f, hitNPCs);
                
                if (nextTarget == null) break;

                hitNPCs.Add(nextTarget.whoAmI);

                DrawLightningBolt(currentTarget.Center, nextTarget.Center);

                nextTarget.SimpleStrikeNPC(chainDamage, 0, false, 0f, null, false, 0f, true);
                nextTarget.AddBuff(ModContent.BuffType<DeltaShock>(), 120);

                for (int i = 0; i < 8; i++)
                {
                    Dust dust = Dust.NewDustDirect(nextTarget.position, nextTarget.width, nextTarget.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 1.5f);
                    dust.noGravity = true;
                    dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                }

                currentTarget = nextTarget;
            }
        }

        private NPC FindNearestEnemy(Vector2 position, float maxDistance, List<int> excludeNPCs)
        {
            NPC closest = null;
            float closestDist = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || excludeNPCs.Contains(i))
                    continue;

                float dist = Vector2.Distance(position, npc.Center);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = npc;
                }
            }

            return closest;
        }

        private void DrawLightningBolt(Vector2 start, Vector2 end)
        {
            int dustCount = (int)(Vector2.Distance(start, end) / 8f);
            for (int i = 0; i < dustCount; i++)
            {
                float progress = i / (float)dustCount;
                Vector2 position = Vector2.Lerp(start, end, progress);
                position += Main.rand.NextVector2Circular(4f, 4f);

                Dust dust = Dust.NewDustPerfect(position, DustID.Electric, Vector2.Zero, 100, Color.White, 2.0f);
                dust.noGravity = true;
            }
        }
    }
}