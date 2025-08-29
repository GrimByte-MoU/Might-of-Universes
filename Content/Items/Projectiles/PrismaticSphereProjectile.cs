using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PrismaticSphereProjectile : ModProjectile
{
    private float rotation = 0f;
    private const float ORBIT_RADIUS = 100f;
    private const float ROTATION_SPEED = 0.15f;

    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        Projectile.damage = 60;
    }
public override bool? CanHitNPC(NPC target) => true;
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
    
    rotation += ROTATION_SPEED;
    float offsetRotation = rotation + (MathHelper.TwoPi * Projectile.ai[0] / 4f); // 4 spheres

    Vector2 offset = new Vector2(
        ORBIT_RADIUS * (float)System.Math.Cos(offsetRotation),
        ORBIT_RADIUS * (float)System.Math.Sin(offsetRotation)
    );

        Projectile.Center = player.Center + offset;
        
        // Rainbow lighting
        Lighting.AddLight(Projectile.Center, Main.DiscoColor.ToVector3() * 0.5f);

        // Rainbow dust
        if (Main.rand.NextBool(2))
        {
            Dust dust = Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.RainbowTorch,
                0f, 0f,
                100,
                default,
                0.5f);
            dust.noGravity = true;
            dust.velocity *= 0.3f;
            dust.fadeIn = 0.1f;
            dust.alpha = 100;
        }
        
        if (!player.GetModPlayer<PrismaticSpherePlayer>().hasPrismaticSphere)
        {
            Projectile.Kill();
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<PrismaticRend>(), 120);
    }
}
}
