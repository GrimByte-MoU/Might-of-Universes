using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class LunarSphereProjectile : MoUProjectile
{
    private float rotation = 0f;
    private const float ORBIT_RADIUS = 125f;
    private const float ROTATION_SPEED = 0.2f;

    public override void SafeSetDefaults()
    {
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        Projectile.damage = 80;
    }
public override bool? CanHitNPC(NPC target) => true;
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
    
    rotation += ROTATION_SPEED;
    float offsetRotation = rotation + (MathHelper.TwoPi * Projectile.ai[0] / 5f); // 5 spheres

    Vector2 offset = new Vector2(
        ORBIT_RADIUS * (float)System.Math.Cos(offsetRotation),
        ORBIT_RADIUS * (float)System.Math.Sin(offsetRotation)
    );

        Projectile.Center = player.Center + offset;

Lighting.AddLight(Projectile.Center, 0.9f, 0.9f, 0.3f);

if (Main.rand.NextBool(2))
{
    Dust dust = Dust.NewDustDirect(
        Projectile.position,
        Projectile.width,
        Projectile.height,
        DustID.PlatinumCoin,
        0f, 0f,
        100,
        default,
        0.8f);
    dust.noGravity = true;
    dust.velocity *= 0.3f;
    dust.fadeIn = 0.1f;
    dust.alpha = 100;

    if (Main.rand.NextBool(3))
    {
        Dust sparkle = Dust.NewDustDirect(
            Projectile.position,
            Projectile.width,
            Projectile.height,
            DustID.WhiteTorch,
            0f, 0f,
            0,
            default,
            0.3f);
        sparkle.noGravity = true;
        sparkle.velocity *= 0.5f;
    }
}

        
        if (!player.GetModPlayer<LunarSpherePlayer>().hasLunarSphere)
        {
            Projectile.Kill();
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<LunarReap>(), 120);
    }
}
}