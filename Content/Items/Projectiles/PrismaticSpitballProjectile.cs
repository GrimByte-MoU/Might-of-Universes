using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles;
public class PrismaticSpitballProjectile : MoUProjectile
{
    public override void SafeSetDefaults()
    {
        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.DamageType = DamageClass.Generic;
        Projectile.damage = 100;
        Projectile.timeLeft = 600;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<PrismaticRend>(), 240);
        target.AddBuff(BuffID.OnFire3, 240);
        target.AddBuff(BuffID.CursedInferno, 240);
    }

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        
        Lighting.AddLight(Projectile.Center, Main.DiscoColor.ToVector3() * 0.5f);

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
        }
    }
}
