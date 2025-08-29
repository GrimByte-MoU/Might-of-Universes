using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TeslaPointerBolt : ModProjectile
    {
        private float curveDirection = 1f;
        private const float CURVE_STRENGTH = 0.1f;
    private float curveStrength = 0.2f;
    
    public override void SetDefaults()
    {
        Projectile.width = 8;
        Projectile.height = 8;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 30;
        Projectile.alpha = 0;
        Projectile.light = 0.5f;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;
    }

    public override void AI()
    {
        // ai1 determines curve direction (1 or -1)
        Projectile.velocity = Projectile.velocity.RotatedBy(curveStrength * Projectile.ai[1] * 
            (1f - Projectile.timeLeft / 60f));
        
        // Create electric effects
        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
            DustID.Electric, 0f, 0f, 100, default, 1f);
    }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 120);
        }
    }
}
