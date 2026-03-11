using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PompeiiFireball : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 2;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.25f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    0f, 0f,
                    100,
                    default,
                    1.5f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 1f, 0.6f, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
           target.AddBuff(ModContent.BuffType<CoreHeat>(), 120);

            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(9f, target.Center);

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0, 0, 100, default, 2f);
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Color.Orange * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 0.5f;
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[i], drawOrigin, Projectile.scale, SpriteEffects.None);
            }

            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}