using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CelestialReaperHeld : MoUProjectile
    {
        private const float MAX_CHARGE_TIME = 240f;
        private const float HALF_CHARGE_TIME = 120f;
        private float chargeTime = 0f;
        private float spinSpeed = 0f;
        private int orbSpawnTimer = 0;
        private bool released = false;

        private float ChargePercent => Math.Min(1f, chargeTime / MAX_CHARGE_TIME);
        private bool IsHalfCharged => chargeTime >= HALF_CHARGE_TIME;
        private bool IsFullyCharged => chargeTime >= MAX_CHARGE_TIME;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 9999;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.channel || player.dead || !player.active)
            {
                if (!released)
                {
                    ReleaseScythe(player);
                }
                return;
            }

            if (!released)
            {
                HoldingBehavior(player);
            }
        }

        private void HoldingBehavior(Player player)
        {
            Projectile.Center = player.Center;
            player.itemTime = 2;
            player.itemAnimation = 2;
            chargeTime = Math.Min(MAX_CHARGE_TIME, chargeTime + 1f);
            float targetSpinSpeed = ChargePercent;
            spinSpeed = MathHelper.Lerp(spinSpeed, targetSpinSpeed, 0.05f);
            float rotationSpeed = 0.1f + (spinSpeed * 0.4f);
            Projectile.rotation += rotationSpeed;
            Vector2 toMouse = Main.MouseWorld - player.Center;
            player.direction = toMouse.X > 0 ? 1 : -1;
            SpawnCelestialOrbs(player);

            if (IsHalfCharged)
            {
                if (Main.rand.NextBool(3))
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 100, default, 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity = Projectile.rotation.ToRotationVector2() * 3f;
                }
            }

            if (IsFullyCharged)
            {
                Projectile.scale = 1f + (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.1f;
                
                if (Main.rand.NextBool(2))
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 100, default, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 2f;
                }
            }
        }

        private void SpawnCelestialOrbs(Player player)
        {
            orbSpawnTimer++;

            int spawnRate;
            if (IsFullyCharged)
            {
                spawnRate = 5;
            }
            else if (IsHalfCharged)
            {
                spawnRate = 10;
            }
            else
            {
                return;
            }

            if (orbSpawnTimer >= spawnRate)
            {
                orbSpawnTimer = 0;
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 spawnOffset = angle.ToRotationVector2() * 40f;
                Vector2 spawnPos = Projectile.Center + spawnOffset;
                NPC target = FindClosestNPC(Projectile.Center, 600f);
                Vector2 velocity;
                
                if (target != null)
                {
                    velocity = target.Center - spawnPos;
                    velocity.Normalize();
                    velocity *= 8f;
                }
                else
                {
                    velocity = angle.ToRotationVector2() * 8f;
                }

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, velocity, 
                    ModContent.ProjectileType<CelestialOrb>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack * 0.5f, player.whoAmI);
            }
        }

        private void ReleaseScythe(Player player)
        {
            released = true;

            Vector2 direction = Main.MouseWorld - player.Center;
            if (direction.LengthSquared() < 0.0001f)
                direction = new Vector2(player.direction, 0f);
            direction.Normalize();
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, direction * 20f,
                ModContent.ProjectileType<CelestialReaperThrown>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 
                IsFullyCharged ? 1f : 0f);

            SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
            Projectile.Kill();
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            ReaperPlayer reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.AddSoulEnergy(10f, target.Center);
            target.AddBuff(BuffID.Daybreak, 180);
            target.AddBuff(BuffID.Frostburn2, 180);
            target.AddBuff(BuffID.Venom, 180);
            target.AddBuff(BuffID.Ichor, 180);
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            if (IsHalfCharged)
            {
                Color glowColor = new Color(100, 200, 255, 0) * (spinSpeed * 0.5f);
                Main.EntitySpriteDraw(texture, drawPos, null, glowColor, Projectile.rotation, drawOrigin, Projectile.scale * 1.2f, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}