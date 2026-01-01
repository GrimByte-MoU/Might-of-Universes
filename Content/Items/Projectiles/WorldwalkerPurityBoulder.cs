using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WorldwalkerPurityBoulder : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 12; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f * Projectile.direction;
            Projectile.velocity.Y += 0.1f;

            // Bounce logic
            if (Projectile.velocity.Y > 16f) Projectile.velocity.Y = 16f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
{
    Projectile.ai[0]++;
    if (Projectile.ai[0] >= 6)  // 6 bounces (up from 3)
        Projectile.Kill();
    else
    {
        // Better bounce retention
        if (Projectile.velocity.X != oldVelocity.X) 
            Projectile.velocity. X = -oldVelocity.X * 0.90f;  // 90% retention
        if (Projectile.velocity.Y != oldVelocity.Y) 
            Projectile.velocity.Y = -oldVelocity. Y * 0.90f;
    }
    return false;
}

// Add lifesteal
public override void OnHitNPC(NPC target, NPC. HitInfo hit, int damageDone)
{
    target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);
}

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Defense *= 0.5f; 
        }
    }
}
