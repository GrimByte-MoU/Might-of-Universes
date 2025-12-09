using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common. Players;

namespace MightofUniverses.Content.Items. Projectiles
{
    public class FleshGlob : MoUProjectile
    {
        private int visualVariant;

        public override void SetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile. penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            visualVariant = Main.rand.Next(3);
        }

        public override void AI()
        {
            Projectile.velocity. Y += 0.3f;
            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;

            Projectile.rotation += Projectile.velocity.X * 0.05f;

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Blood, 0f, 0f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player owner = Main.player[Projectile.owner];
            var reaperPlayer = owner.GetModPlayer<ReaperPlayer>();
            reaperPlayer.AddSoulEnergy(2);

            for (int i = 0; i < 8; i++)
            {
                Dust. NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Blood, Projectile.velocity.X * 0.3f, Projectile.velocity. Y * 0.3f);
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID. Blood, 0f, 0f, 100, default, 1.5f);
            }
        }
    }
}