using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items. Buffs;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TerraiumStaffOrb : MoUProjectile
    {

        private int chargeLevel = 0;
        private const int maxChargeLevel = 4;
        private int chargeTicks = 0;

        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile. timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (! player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            if (player. channel && chargeLevel < maxChargeLevel)
            {
                chargeTicks++;

                if (chargeTicks >= 30)
                {
                    chargeLevel++;
                    chargeTicks = 0;

                    Terraria.Audio.SoundEngine. PlaySound(SoundID.Item29, Projectile.Center);

                    for (int i = 0; i < 20; i++)
                    {
                        float angle = i / 20f * MathHelper.TwoPi;
                        Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (30f + chargeLevel * 10f);
                        Dust dust = Dust. NewDustPerfect(Projectile. Center + offset, DustID. TerraBlade, Vector2.Zero, 100, Color.LimeGreen, 2.0f + chargeLevel * 0.5f);
                        dust.noGravity = true;
                    }
                }

                Projectile.Center = player.Center;
                Projectile.velocity = Vector2.Zero;
                player.itemTime = 2;
                player.itemAnimation = 2;

                Projectile.rotation += 0.1f;
                Projectile. scale = 1f + (chargeLevel * 0.3f);

                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID. TerraBlade, 0, 0, 100, Color. LimeGreen, 1.5f + chargeLevel * 0.3f);
                    dust.noGravity = true;
                    dust.velocity = (Projectile.Center - dust.position).SafeNormalize(Vector2.UnitY) * -2f;
                }

                Lighting.AddLight(Projectile.Center, 0.3f + chargeLevel * 0.2f, 0.8f + chargeLevel * 0.1f, 0.3f + chargeLevel * 0.2f);
            }
            else
            {
                if (Projectile.velocity == Vector2.Zero)
                {
                    Vector2 direction = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);
                    Projectile.velocity = direction * 18f;
                    Projectile. friendly = true;
                    Projectile.penetrate = 1;
                    Projectile. timeLeft = 300;

                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item84, Projectile.Center);

                    for (int i = 0; i < 30; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.5f);
                        dust.noGravity = true;
                        dust.velocity = direction. RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) * Main.rand.NextFloat(3f, 8f);
                    }
                }

                NPC target = FindClosestNPC(800f);
                if (target != null)
                {
                    Vector2 direction = (target.Center - Projectile. Center).SafeNormalize(Vector2.UnitX);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 18f, 0.05f);
                }

                Projectile.rotation += 0.3f;

                if (Main. rand.NextBool())
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.0f);
                    dust.noGravity = true;
                    dust.velocity *= 0.3f;
                }

                Lighting.AddLight(Projectile.Center, 0.6f, 1.0f, 0.6f);
            }
        }

        private NPC FindClosestNPC(float maxDistance)
        {
            NPC closestNPC = null;
            float closestDist = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                
                if (! npc.active)
                    continue;
                    
                if (npc.friendly)
                    continue;
                    
                if (npc. dontTakeDamage)
                    continue;
                
                // IGNORE TARGET DUMMIES - ADD THIS LINE
                if (npc.type == NPCID.TargetDummy)
                    continue;

                float distance = Vector2.Distance(Projectile.Center, npc. Center);
                if (distance < closestDist)
                {
                    closestDist = distance;
                    closestNPC = npc;
                }
            }

            return closestNPC;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.LimeGreen;
        }

        public override void ModifyHitNPC(NPC target, ref NPC. HitModifiers modifiers)
        {
            float damageMultiplier = (float)Math.Pow(2, chargeLevel);
            modifiers. SourceDamage *= damageMultiplier;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);

            for (int i = 0; i < 25 + (chargeLevel * 10); i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.5f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f));
            }

            Terraria. Audio.SoundEngine.PlaySound(SoundID.Item62, target.Center);
        }
    }
}