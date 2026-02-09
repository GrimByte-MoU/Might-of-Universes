using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna. Framework;
using MightofUniverses.Content.Items. Buffs;
using System.Collections.Generic;
using System. Linq;

namespace MightofUniverses.Content.Items. Projectiles
{
    public class TerraiumRifleShot : ModProjectile
    {
        private int lastHitNPC = -1;
        private int bounceCount = 0;
        private const int maxBounces = 10;

        public override void SetDefaults()
        {
            Projectile. width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile. DamageType = DamageClass.Ranged;
            Projectile. penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile. tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 1.5f);
                dust. noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.8f, 0.3f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.LimeGreen;
        }

        public override bool?  CanHitNPC(NPC target)
        {
            if (target.whoAmI == lastHitNPC)
                return false;

            return base.CanHitNPC(target);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FlatBonusDamage += 50;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);

            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target. width, target.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.0f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
            }

            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Projectile.Kill();
                return;
            }

            NPC nextTarget = FindNextTarget(target);
            if (nextTarget != null)
            {
                lastHitNPC = target.whoAmI;

                Vector2 direction = (nextTarget.Center - Projectile. Center).SafeNormalize(Vector2.UnitX);
                float currentSpeed = Projectile.velocity.Length();
                Projectile.velocity = direction * currentSpeed;

                for (int i = 0; i < 15; i++)
                {
                    Dust dust = Dust. NewDustDirect(Projectile.Center, Projectile. width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.Yellow, 2.0f);
                    dust.noGravity = true;
                    dust.velocity = direction * 4f;
                }

                Terraria. Audio.SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
            }
            else
            {
                Projectile.Kill();
            }
        }

        private NPC FindNextTarget(NPC currentTarget)
        {
            float searchRadius = 800f;
            NPC closestNPC = null;
            float closestDistance = searchRadius;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (! npc.active)
                    continue;

                if (npc.friendly)
                    continue;

                if (npc.dontTakeDamage)
                    continue;

                if (npc.whoAmI == currentTarget.whoAmI)
                    continue;

                float distance = Vector2.Distance(Projectile.Center, npc.Center);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNPC = npc;
                }
            }

            return closestNPC;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust. NewDustDirect(Projectile.position, Projectile. width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 1.2f);
                dust.noGravity = true;
            }
            return true;
        }
    }
}