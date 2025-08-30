using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    // Basic custom whip projectile for Whipsword's whip stance.
    public class WhipswordWhipProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
            Projectile.scale = 1.25f;
        }

        public override void SetDefaults()
        {
            // Sets up most whip behavior (aiStyle, friendly, local immunity, etc.)
            Projectile.DefaultToWhip();

            // Core whip tuning
            Projectile.WhipSettings.Segments = 22;           // number of links
            Projectile.WhipSettings.RangeMultiplier = 1.1f;  // reach

            Projectile.DamageType = DamageClass.SummonMeleeSpeed;
        }

        // Optional: small visual flair on the tip during AI
        public override void AI()
        {
            // Light golden/bone vibe
            Lighting.AddLight(Projectile.Center, 0.12f, 0.10f, 0.04f);

            if (Main.rand.NextBool(6))
            {
                int dustType = Main.rand.NextBool() ? DustID.Bone : DustID.GoldFlame;

                var controlPoints = new System.Collections.Generic.List<Vector2>();
                Projectile.FillWhipControlPoints(Projectile, controlPoints);
                Vector2 tip = controlPoints.Count > 0 ? controlPoints[controlPoints.Count - 1] : Projectile.Center;

                var d = Dust.NewDustDirect(tip - new Vector2(2), 4, 4, dustType, 0f, 0f, 100, default, 1.0f);
                d.noGravity = true;
                d.velocity *= 0.2f;
            }
        }

        // Whips commonly apply a tag debuff; left blank to keep it generic.
        // public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
    }
}