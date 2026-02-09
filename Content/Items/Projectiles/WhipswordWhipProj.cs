using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WhipswordWhipProj : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
            Projectile.scale = 1.25f;
        }

        public override void SafeSetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 22;
            Projectile.WhipSettings.RangeMultiplier = 1.1f;
            Projectile.DamageType = DamageClass.SummonMeleeSpeed;
        }

        public override void AI()
        {
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
    }
}