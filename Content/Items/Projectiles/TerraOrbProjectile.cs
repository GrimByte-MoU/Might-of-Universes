using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna. Framework;
using MightofUniverses.Content.Items.Buffs;
using System;

namespace MightofUniverses. Content.Items.Projectiles
{
    public class TerraOrbProjectile : ModProjectile
    {

        private int hitCount = 0;
        private const int maxHits = 5;
        private int leafShootTimer = 0;

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = maxHits;
            Projectile. timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override bool?  CanDamage()
        {
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (! player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (other.active && other.type == Projectile.type && other.owner == Projectile.owner && other.whoAmI < Projectile.whoAmI)
                {
                    Projectile.Kill();
                    return;
                }
            }

            Vector2 targetPosition = Main.MouseWorld;
            Vector2 direction = (targetPosition - Projectile.Center).SafeNormalize(Vector2.UnitX);
            float distance = Vector2.Distance(Projectile.Center, targetPosition);

            if (distance > 20f)
            {
                Projectile.velocity = direction * Math.Min(distance * 0.3f, 20f);
            }
            else
            {
                Projectile.velocity *= 0.9f;
            }

            Projectile.rotation += 0.2f;

            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.5f);
                dust. noGravity = true;
                dust.velocity = (Projectile.Center - dust.position).SafeNormalize(Vector2.UnitY) * -3f;
            }

            leafShootTimer++;
            if (leafShootTimer >= 15)
            {
                leafShootTimer = 0;
                ShootLeaves();
            }

            Lighting.AddLight(Projectile.Center, 0.6f, 1.2f, 0.6f);
        }

        private void ShootLeaves()
        {
            NPC target = FindClosestNPC(600f);
            if (target == null)
                return;

            float spreadAngle = MathHelper.ToRadians(5);

            for (int i = -1; i <= 1; i++)
            {
                Vector2 baseDirection = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);
                float angle = baseDirection.ToRotation() + (i * spreadAngle / 2f);
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 16f;

                int leafDamage = (int)(Projectile.damage * 0.5f);

                Projectile.NewProjectile(
                    Projectile. GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<TerraLeaf>(),
                    leafDamage,
                    2f,
                    Projectile.owner
                );
            }
        }

        private NPC FindClosestNPC(float maxDistance)
        {
            NPC closestNPC = null;
            float closestDist = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDist)
                    {
                        closestDist = distance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.LimeGreen;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= 2.0f;
        }

        public override void OnHitNPC(NPC target, NPC. HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);

            hitCount++;

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust. NewDustDirect(target. position, target.width, target. height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.5f);
                dust.noGravity = true;
                dust. velocity = new Vector2(Main. rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
            }

            if (hitCount >= maxHits)
            {
                Explode();
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (hitCount >= maxHits)
            {
                Explode();
            }
        }

        private void Explode()
        {
            float explosionRadius = 160f;
            int explosionDamage = (int)(Projectile.damage * 2.5f);

            for (int ring = 0; ring < 5; ring++)
            {
                float currentRadius = explosionRadius / 5 * (ring + 1);
                int segments = 20 + (ring * 8);

                for (int i = 0; i < segments; i++)
                {
                    float angle = (i / (float)segments) * MathHelper.TwoPi;
                    Vector2 position = Projectile.Center + new Vector2(
                        (float)Math.Cos(angle) * currentRadius,
                        (float)Math.Sin(angle) * currentRadius
                    );

                    int dustIndex = Dust.NewDust(position, 0, 0, DustID.TerraBlade, 0, 0, 100, Color.Green, 2.5f);
                    Main.dust[dustIndex].noGravity = true;
                    Main. dust[dustIndex].velocity = new Vector2(
                        (float)Math.Cos(angle) * 3f,
                        (float)Math.Sin(angle) * 3f
                    );
                    Main.dust[dustIndex].scale = 2.0f + (ring * 0.3f);
                }
            }

            for (int i = 0; i < 50; i++)
            {
                Vector2 velocity = new Vector2(
                    Main.rand. NextFloat(-10f, 10f),
                    Main.rand.NextFloat(-10f, 10f)
                );
                int dustIndex = Dust.NewDust(Projectile.Center, 30, 30, DustID.TerraBlade, velocity.X, velocity.Y, 0, Color.LimeGreen, 3.0f);
                Main.dust[dustIndex].noGravity = true;
            }

            Lighting. AddLight(Projectile.Center, 1.5f, 2.5f, 1.5f);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main. npc[i];

                if (! npc.active)
                    continue;

                if (npc.friendly)
                    continue;

                if (npc. dontTakeDamage)
                    continue;

                float distance = Vector2.Distance(Projectile.Center, npc.Center);

                if (distance <= explosionRadius)
                {
                    int savedDefense = npc.defense;
                    npc.defense = 0;
                    npc.SimpleStrikeNPC(explosionDamage, 0, false, 0, null, false, 0, true);
                    npc. defense = savedDefense;

                    npc.AddBuff(ModContent.BuffType<TerrasRend>(), 240);

                    for (int d = 0; d < 12; d++)
                    {
                        Dust dust = Dust. NewDustDirect(npc.position, npc.width, npc.height, DustID.TerraBlade, 0, 0, 100, Color.Green, 2.0f);
                        dust.noGravity = true;
                        dust.velocity = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f));
                    }
                }
            }
        }
    }
}