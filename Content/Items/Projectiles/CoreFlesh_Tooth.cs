using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    // Fast, gravity-affected tooth projectile.
    // - No pierce (penetrate = 1)
    // - Grants +2 souls to the owner on hit (handled here)
    // - Gravity toggle via ai[1] (1 = gravity on)
    public class CoreFlesh_Tooth : ModProjectile
    {
        // Tweakable gravity and speed caps
        private const float Gravity = 0.35f;
        private const float MaxFallSpeed = 18f;

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;              
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 300;              
            Projectile.extraUpdates = 0;            
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.aiStyle = 0;                
        }

        public override void AI()
        {
            if (Projectile.ai[1] != 0f)
            {
                Projectile.velocity.Y += Gravity;
                if (Projectile.velocity.Y > MaxFallSpeed)
                    Projectile.velocity.Y = MaxFallSpeed;
            }
            if (Projectile.velocity.LengthSquared() > 0.001f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            if (Main.rand.NextBool(18))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 0.9f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.3f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Simple shatter on impact
            Projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player owner = Main.player[Projectile.owner];
            if (owner?.active == true && !owner.dead)
            {
                if (Main.myPlayer == owner.whoAmI)
                {
                    var reaper = owner.GetModPlayer<ReaperPlayer>();
                    reaper.soulEnergy += 2;
                }
            }
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            // Shatter dust
            for (int i = 0; i < 8; i++)
            {
                Vector2 v = Main.rand.NextVector2Circular(2.5f, 2.5f);
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, v.X, v.Y, 100, default, 1.1f);
                Main.dust[d].noGravity = true;
            }

        }
    }
}