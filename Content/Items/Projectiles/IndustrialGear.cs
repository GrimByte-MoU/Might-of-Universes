using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class IndustrialGear : ModProjectile
    {
        private bool isSpinning = false;
        private int spinTimer = 0;
        private const int SPIN_DURATION = 180;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            Projectile.rotation += 0.6f;

            if (!isSpinning)
            {
                Projectile.velocity *= 0.98f;
                if (Projectile.velocity.Length() < 1f)
                {
                    isSpinning = true;
                    Projectile.velocity = Vector2.Zero;
                }
            }
            else
            {
                spinTimer++;
                if (spinTimer >= SPIN_DURATION)
                    Projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            
            target.AddBuff(ModContent.BuffType<Shred>(), 120);
            reaperPlayer.AddSoulEnergy(0.2f, target.Center);
        }
    }
}
