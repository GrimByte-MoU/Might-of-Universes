using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MegaBusterChargeLevel2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 20;
            Projectile.timeLeft = 300;
            Projectile.light = 1.5f;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            float scale = Projectile.ai[0];
            if (scale > 0)
            {
                Projectile.scale = scale;
            }
            
            if (Main.rand.NextBool(1))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 2.0f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, Color.LightCyan, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
            
            Lighting.AddLight(Projectile.Center, 0.3f, 1f, 1f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Cyan;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 20; i++)
            {
                Color dustColor = Main.rand.NextBool() ? Color.Cyan : Color.LightCyan;
                int dustType = Main.rand.NextBool() ? DustID.Electric : DustID.IceTorch;
                
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, dustType, 0f, 0f, 100, dustColor, 2.2f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
            }
            
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, target.Center);
        }
    }
}