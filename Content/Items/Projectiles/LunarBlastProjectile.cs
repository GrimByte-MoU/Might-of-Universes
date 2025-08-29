using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles;
public class LunarBlastProjectile : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        Projectile.timeLeft = 600;
        Projectile.tileCollide = false;
        Projectile.damage = 125;
    }

    public override void AI()
    {
        NPC target = null;
        float maxDistance = 1000f;

        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC npc = Main.npc[i];
            if (npc.active && !npc.friendly)
            {
                float distance = Vector2.Distance(Projectile.Center, npc.Center);
                if (distance < maxDistance)
                {
                    maxDistance = distance;
                    target = npc;
                }
            }
        }

        if (target != null)
        {
            Vector2 direction = target.Center - Projectile.Center;
            direction.Normalize();
            float speed = 15f;
            Projectile.velocity = (Projectile.velocity * 20f + direction * speed) / 21f;
        }

        Projectile.rotation = Projectile.velocity.ToRotation();
        
        Lighting.AddLight(Projectile.Center, 0.9f, 0.9f, 0.3f);

        if (Main.rand.NextBool(2))
        {
            Dust dust = Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.LunarOre,
                0f, 0f,
                100,
                default,
                1f);
            dust.noGravity = true;
            dust.velocity *= 0.3f;
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<LunarReap>(), 180);
        target.AddBuff(BuffID.Ichor, 180);
        target.AddBuff(BuffID.CursedInferno, 180);
    }
}
