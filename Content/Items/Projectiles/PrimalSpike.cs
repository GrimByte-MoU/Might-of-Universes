using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PrimalSpike : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        private bool isLaunched = false;
        private const float OrbitRadius = 40f;
        private const float LaunchSpeed = 25f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.GetModPlayer<PrimalPlatePlayer>().hasPrimalPlateSet)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.ai[1] == 1f && !isLaunched)
            {
                LaunchSpike(player);
                return;
            }

            if (isLaunched)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity *= 0.98f;
                Projectile.timeLeft--;
                if (Projectile.timeLeft <= 0)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                float spikeIndex = Projectile.ai[0];
                float totalSpikes = 8f;
                
                // Get the global rotation from player
                var primalPlayer = player.GetModPlayer<PrimalPlatePlayer>();
                float angle = (spikeIndex / totalSpikes) * MathHelper.TwoPi;
                angle += primalPlayer.globalSpikeRotation; // Use shared rotation
                
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle) * OrbitRadius,
                    (float)Math.Sin(angle) * OrbitRadius
                );
                
                Projectile.Center = player.Center + offset;
                Vector2 awayDirection = Projectile.Center - player.Center;
                //Projectile.rotation = awayDirection.ToRotation();
                Projectile.timeLeft = 60;
            }

            if (Main.rand.NextBool(4))
            {
                Color dustColor = Main.rand.Next(3) switch
                {
                    0 => new Color(139, 90, 43),  // Brown/earth
                    1 => new Color(101, 67, 33),  // Dark brown
                    _ => new Color(218, 165, 32)  // Amber/gold
                };
                
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ambient_DarkBrown, 0f, 0f, 100, dustColor, 1.0f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
            Lighting.AddLight(Projectile.Center, 0.6f, 0.4f, 0.1f);
        }

        private void LaunchSpike(Player player)
        {
            isLaunched = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            
            // Get direction to cursor
            Vector2 directionToCursor = Main.MouseWorld - Projectile.Center;
            directionToCursor.Normalize();
            
            Projectile.velocity = directionToCursor * LaunchSpeed;
            
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Apply Tarred debuff for 5 seconds
            target.AddBuff(ModContent.BuffType<Tarred>(), 300);
            
            // Set respawn timer when this spike dies (ALWAYS 5 seconds whether orbiting or launched)
            var primalPlayer = Main.player[Projectile.owner].GetModPlayer<PrimalPlatePlayer>();
            int spikeIndex = (int)Projectile.ai[0];
            if (spikeIndex >= 0 && spikeIndex < 8)
            {
                primalPlayer.spikeRespawnTimers[spikeIndex] = 300; // 5 seconds (60 fps * 5)
            }
            
            // Visual impact effect
            for (int i = 0; i < 10; i++)
            {
                Color dustColor = Main.rand.Next(2) switch
                {
                    0 => new Color(139, 90, 43),
                    _ => new Color(0, 0, 0) // Black tar
                };
                
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ambient_DarkBrown, 0f, 0f, 100, dustColor, 1.5f);
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
                dust.noGravity = true;
            }
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (isLaunched)
            {
                // Set respawn timer when hitting tiles too
                var primalPlayer = Main.player[Projectile.owner].GetModPlayer<PrimalPlatePlayer>();
                int spikeIndex = (int)Projectile.ai[0];
                if (spikeIndex >= 0 && spikeIndex < 8)
                {
                    primalPlayer.spikeRespawnTimers[spikeIndex] = 300; // 5 seconds
                }
                return true;
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Bone, 0f, 0f, 100, new Color(139, 90, 43), 1.2f);
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
            }
        }
    }
}