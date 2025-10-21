using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ArcaneGladiusProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
            Vector2 target = Main.MouseWorld;
            Vector2 toTarget = target - Projectile.Center;
            float desiredSpeed = Projectile.velocity.Length();

            if (toTarget.Length() > 32f)
            {
                toTarget.Normalize();
                Vector2 desiredVelocity = toTarget * desiredSpeed;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void OnSpawn(IEntitySource source)
{
    Player player = Main.player[Projectile.owner];

    // Get both melee and magic bonuses
    float meleeBoost = player.GetDamage(DamageClass.Melee).Additive;
    float magicBoost = player.GetDamage(DamageClass.Magic).Additive;
    float totalBoost = 1f + meleeBoost + magicBoost;

    Projectile.damage = (int)(Projectile.damage * totalBoost);
}
    }
}