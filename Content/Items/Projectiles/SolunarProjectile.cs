using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SolunarProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 2;
            Projectile.timeLeft = 180;
            Projectile.light = 1f;
        }

        public override void AI()
        {    
            // Helix movement
            Projectile.rotation = Projectile.velocity.ToRotation();
            float waveSpeed = 0.5f;
            float waveWidth = 20f;
            Vector2 offset = new Vector2(0, (float)Math.Sin(Projectile.timeLeft * waveSpeed) * waveWidth);
            offset = offset.RotatedBy(Projectile.velocity.ToRotation());
            Projectile.position += offset * Projectile.ai[0];

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch);
            Lighting.AddLight(Projectile.Center, 0.8f, 0f, 0.8f);
            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(1f, target.Center);
        }
    }
}
