using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;      // ReaperAccessoryPlayer.ReportPassiveSoulGain
using MightofUniverses.Content.Items.Buffs; // CagedSoul

namespace MightofUniverses.Content.Items.Projectiles
{
    // Specter Orb (Spectercage Artifact): homes to the owner, grants 5 souls, heals 10, applies Caged Soul for 3s.
    public class SpecterOrbProj : MoUProjectile
    {
        private const int SoulsGranted = 5;
        private const int HealAmount = 10;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            int owner = Projectile.owner;
            if (owner < 0 || owner >= Main.maxPlayers) { Projectile.Kill(); return; }
            Player player = Main.player[owner];
            if (player == null || !player.active) { Projectile.Kill(); return; }

            // Subtle bob + easing
            Projectile.rotation += 0.02f * Projectile.direction;
            float bob = (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 6f + Projectile.whoAmI) * 0.3f;
            Projectile.velocity *= 0.98f;
            Projectile.velocity.Y += bob * 0.02f;

            // Home to player
            Vector2 to = player.Center - Projectile.Center;
            float dist = to.Length();
            if (dist > 6f)
            {
                to.Normalize();
                float speed = MathHelper.Lerp(7f, 13f, 1f - MathHelper.Clamp(dist / 520f, 0f, 1f));
                Vector2 desired = to * speed;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, 0.12f);
            }

            // Ghostly light
            Lighting.AddLight(Projectile.Center, new Vector3(0.35f, 0.75f, 0.95f) * 0.8f);

            // Sparkle trail
            if (Main.rand.NextBool(4))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonSpirit,
                    0f, 0f, 150, new Color(180, 240, 255), 1.1f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.3f;
            }

            // Pickup
            if (dist <= 20f)
            {
                ReaperAccessoryPlayer.ReportPassiveSoulGain(player, SoulsGranted);

                player.Heal(HealAmount);
                player.AddBuff(ModContent.BuffType<CagedSoulBuff>(), 180);

                // Pickup burst
                for (int i = 0; i < 12; i++)
                {
                    Vector2 v = Main.rand.NextVector2Circular(2.4f, 2.4f);
                    int d = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 8, 8, DustID.DungeonSpirit, v.X, v.Y, 100, new Color(180, 240, 255), 1.2f);
                    Main.dust[d].noGravity = true;
                }

                Projectile.Kill();
            }
        }
    }
}