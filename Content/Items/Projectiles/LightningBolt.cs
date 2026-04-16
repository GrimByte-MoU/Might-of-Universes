using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class LightningBolt : MoUProjectile
    {
        private List<Vector2> boltPoints = new List<Vector2>();
        private bool initialized = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 15;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (!initialized)
            {
                GenerateLightningPath();
                CreateLightningDust();
                initialized = true;
            }
        }

        private void GenerateLightningPath()
        {
            Vector2 skyPosition = new Vector2(Projectile.Center.X, Projectile.Center.Y - 800f);
            Vector2 currentPos = skyPosition;
            boltPoints.Add(currentPos);

            Vector2 targetPos = Projectile.Center;
            float totalDistance = Vector2.Distance(skyPosition, targetPos);
            int segments = 16;
            float segmentLength = totalDistance / segments;

            Vector2 mainDirection = targetPos - skyPosition;
            mainDirection.Normalize();

            for (int i = 0; i < segments; i++)
            {
                float lateralOffset = Main.rand.NextFloat(-25f, 25f);
                Vector2 perpendicular = new Vector2(-mainDirection.Y, mainDirection.X);
                Vector2 offset = mainDirection * segmentLength + perpendicular * lateralOffset;
                
                currentPos += offset;
                boltPoints.Add(currentPos);
            }

            boltPoints[boltPoints.Count - 1] = targetPos;
        }

        private void CreateLightningDust()
        {
            for (int i = 0; i < boltPoints.Count - 1; i++)
            {
                Vector2 start = boltPoints[i];
                Vector2 end = boltPoints[i + 1];
                Vector2 direction = end - start;
                float distance = direction.Length();
                direction.Normalize();

                int dustCount = (int)(distance / 2f);
                for (int j = 0; j < dustCount; j++)
                {
                    float progress = j / (float)dustCount;
                    Vector2 dustPos = start + direction * distance * progress;
                    
                    for (int k = 0; k < 3; k++)
                    {
                        Vector2 offset = Main.rand.NextVector2Circular(3f, 3f);
                        int dustIndex = Dust.NewDust(dustPos + offset, 1, 1, DustID.Electric, 0f, 0f, 100, Color.Cyan, Main.rand.NextFloat(2.5f, 4f));
                        Dust dust = Main.dust[dustIndex];
                        dust.noGravity = true;
                        dust.velocity = Vector2.Zero;
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= 2.5f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DeltaShock>(), 180);
        }

        public override bool? CanDamage() => Projectile.timeLeft > 5;
    }
}