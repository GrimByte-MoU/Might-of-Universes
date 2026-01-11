using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items. Buffs;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class AncientSphere : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile. penetrate = 4;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f;

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 10f)
            {
                Projectile.velocity.Y += 0.05f;
            }

            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID. TerraBlade, 0, 0, 100, Color. LimeGreen, 2.0f);
                dust. noGravity = true;
                dust.velocity *= 0.5f;
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 1.0f, 0.5f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.LimeGreen;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);

            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.0f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f));
            }

            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Explode();
                Projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            if (timeLeft <= 0)
            {
                Explode();
            }
        }

        private void Explode()
        {
            float explosionRadius = 240f;
            int explosionDamage = (int)(Projectile.damage * 2.0f);

            for (int ring = 0; ring < 6; ring++)
            {
                float currentRadius = (explosionRadius / 6) * (ring + 1);
                int segments = 25 + (ring * 10);

                for (int i = 0; i < segments; i++)
                {
                    float angle = (i / (float)segments) * MathHelper.TwoPi;
                    Vector2 position = Projectile.Center + new Vector2(
                        (float)Math.Cos(angle) * currentRadius,
                        (float)Math.Sin(angle) * currentRadius
                    );

                    int dustIndex = Dust.NewDust(position, 0, 0, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.5f);
                    Main.dust[dustIndex].noGravity = true;
                    Main. dust[dustIndex].velocity = new Vector2(
                        (float)Math.Cos(angle) * 3f,
                        (float)Math.Sin(angle) * 3f
                    );
                    Main.dust[dustIndex].scale = 2.0f + (ring * 0.4f);
                }
            }

            for (int i = 0; i < 60; i++)
            {
                Vector2 velocity = new Vector2(
                    Main.rand.NextFloat(-10f, 10f),
                    Main.rand.NextFloat(-10f, 10f)
                );
                int dustIndex = Dust.NewDust(Projectile.Center, 30, 30, DustID. TerraBlade, velocity.X, velocity.Y, 0, Color.Green, 3.0f);
                Main. dust[dustIndex].noGravity = true;
            }

            Lighting.AddLight(Projectile. Center, 1.5f, 2.0f, 1.5f);
            Terraria.Audio.SoundEngine.PlaySound(SoundID. Item14, Projectile.Center);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

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
                    npc.defense = savedDefense;

                    npc.AddBuff(ModContent.BuffType<TerrasRend>(), 120);

                    for (int d = 0; d < 10; d++)
                    {
                        Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.0f);
                        dust.noGravity = true;
                        dust.velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                    }

                    Vector2 direction = (npc.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);
                    for (int d = 0; d < 12; d++)
                    {
                        Vector2 dustPos = Projectile.Center + (direction * distance * (d / 12f));
                        Dust. NewDust(dustPos, 4, 4, DustID. TerraBlade, 0, 0, 100, Color. Green, 1.5f);
                    }
                }
            }
        }
    }
}