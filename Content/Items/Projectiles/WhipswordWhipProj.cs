using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WhipswordWhipProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 18;
            Projectile.WhipSettings.RangeMultiplier = 1.3f;
            Projectile.DamageType = DamageClass.SummonMeleeSpeed;
            Projectile.scale = 1.25f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.75f);
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.12f, 0.10f, 0.04f);

            if (Main.rand.NextBool(6))
            {
                int dustType = Main.rand.NextBool() ? DustID.Bone : DustID.GoldFlame;

                var controlPoints = new List<Vector2>();
                Projectile.FillWhipControlPoints(Projectile, controlPoints);
                Vector2 tip = controlPoints.Count > 0 ? controlPoints[controlPoints.Count - 1] : Projectile.Center;

                var d = Dust.NewDustDirect(tip - new Vector2(2), 4, 4, dustType, 0f, 0f, 100, default, 1.0f);
                d.noGravity = true;
                d.velocity *= 0.2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 pos = list[0];

            for (int i = 0; i < list.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, texture.Width, 26);
                Vector2 origin = new Vector2(texture.Width / 2f, 8);
                float scale = Projectile.scale;

                if (i == list.Count - 2)
                {
                    frame.Y = texture.Height - 18;
                    frame.Height = 18;

                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Projectile.ai[0] / timeToFlyOut;
                    scale *= MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 0)
                {
                    frame.Y = 26;
                    frame.Height = 26;
                }

                Vector2 diff = list[i + 1] - list[i];
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(list[i].ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

                pos += diff;
            }

            return false;
        }
    }
}