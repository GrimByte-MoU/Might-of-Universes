using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PompeiiMeteor : MoUProjectile
    {
        private List<int> hitNPCs = new List<int>();
        private int currentTarget = -1;
        private const int LIFETIME = 180;
        private const float SEARCH_RADIUS = 600f;
        private int searchCooldown = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = LIFETIME;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.scale = 1.5f;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.rotation += 0.3f;

            if (Main.rand.NextBool(1))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    0f, 0f,
                    100,
                    Color.OrangeRed,
                    2.5f
                );
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            Lighting.AddLight(Projectile.Center, 2f, 0.8f, 0f);

            searchCooldown--;
            if (searchCooldown <= 0)
            {
                searchCooldown = 5;
                currentTarget = FindNextEnemy();
            }

            if (currentTarget >= 0 && Main.npc[currentTarget].active && !Main.npc[currentTarget].dontTakeDamage)
            {
                NPC target = Main.npc[currentTarget];
                Vector2 toTarget = target.Center - Projectile.Center;
                float distance = toTarget.Length();

                if (distance > 50f)
                {
                    toTarget.Normalize();
                    Projectile.velocity = toTarget * 25f;
                }
                else
                {
                    if (!hitNPCs.Contains(currentTarget))
                    {
                        hitNPCs.Add(currentTarget);
                    }
                    currentTarget = -1;
                    searchCooldown = 0;
                }
            }
            else
            {
                if (FindNextEnemy() < 0)
                {
                    Projectile.velocity *= 0.95f;
                }
            }

            if (Projectile.timeLeft <= 0)
            {
                Projectile.Kill();
            }
        }

        private int FindNextEnemy()
        {
            float closestDist = SEARCH_RADIUS;
            int closestIndex = -1;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || npc.friendly || npc.lifeMax <= 5 || npc.dontTakeDamage || !npc.CanBeChasedBy())
                    continue;

                if (hitNPCs.Contains(i))
                    continue;

                float dist = Vector2.Distance(Projectile.Center, npc.Center);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestIndex = i;
                }
            }

            if (closestIndex < 0)
            {
                hitNPCs.Clear();
                
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.active || npc.friendly || npc.lifeMax <= 5 || npc.dontTakeDamage || !npc.CanBeChasedBy())
                        continue;

                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestIndex = i;
                    }
                }
            }

            return closestIndex;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CoreHeat>(), 240);

            if (!hitNPCs.Contains(target.whoAmI))
            {
                hitNPCs.Add(target.whoAmI);
            }

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 0.8f, Pitch = -0.2f }, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    velocity.X, velocity.Y,
                    100,
                    Color.OrangeRed,
                    3f
                );
                dust.noGravity = true;
            }

            if (Main.LocalPlayer.Distance(Projectile.Center) < 800f)
            {
                Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>()?.AddShake(6f);
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                
                Color trailColor = Color.Lerp(Color.OrangeRed, Color.Yellow, i / (float)Projectile.oldPos.Length);
                trailColor *= alpha * 0.6f;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    trailColor,
                    Projectile.oldRot[i],
                    drawOrigin,
                    Projectile.scale * 0.9f,
                    SpriteEffects.None,
                    0
                );
            }

            Color glowColor = Color.White * 0.4f;
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale * 1.3f,
                SpriteEffects.None,
                0
            );

            Color mainColor = Color.White;
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                mainColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
            
            for (int i = 0; i < 50; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(12f, 12f);
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    velocity.X, velocity.Y,
                    100,
                    Color.OrangeRed,
                    3f
                );
                dust.noGravity = true;
            }
        }
    }
}