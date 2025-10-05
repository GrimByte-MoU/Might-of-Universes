using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WorldwalkerFireWave : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            // Constant forward velocity
            if (Projectile.velocity.Length() < 8f)
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 8f;
            Projectile.gfxOffY = -32f;

            // Light & dust effect
            Lighting.AddLight(Projectile.Center, 1f, 0.4f, 0.1f);
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f, 150, default, 1.2f);
                Main.dust[dust].noGravity = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Apply Demonfire & Hellfire
            target.AddBuff(ModContent.BuffType<Demonfire>(), 300); // Custom buff (-75 HP/sec)
            target.AddBuff(BuffID.OnFire3, 300); // Hellfire
            Projectile.damage = (int)(Projectile.damage * 0.9f);
        }
    }
}
