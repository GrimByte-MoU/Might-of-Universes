using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ArcaneGladiusProjectile : ModProjectile
    {
        private Vector2 targetPosition;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.timeLeft > 540)
            {
                // Spiral out phase
                Projectile.velocity += new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.One) * 3f;
            }
            else
            {
                // Homing toward cursor
                Vector2 destination = Main.MouseWorld;
                Vector2 direction = destination - Projectile.Center;
                direction.Normalize();
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 16f, 0.15f);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];

            // Apply both Melee and Magic bonuses manually
            float meleeBoost = player.GetDamage(DamageClass.Melee).Additive;
            float magicBoost = player.GetDamage(DamageClass.Magic).Additive;
            float totalBoost = meleeBoost + magicBoost;

            Projectile.damage = (int)(Projectile.damage * totalBoost);
        }

public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
{
    target.AddBuff(ModContent.BuffType<MortalWound>(), 180);
}


    }
}
