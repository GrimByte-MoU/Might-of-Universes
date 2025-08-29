using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ConecussionWhip : ModProjectile
    {
        public override void SetStaticDefaults()
            => ProjectileID.Sets.IsAWhip[Type] = true;

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 7;
            Projectile.WhipSettings.RangeMultiplier = 1f;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.HasBuff(ModContent.BuffType<Hyper>()))
            {
                Projectile.extraUpdates = 2;
                Projectile.damage = (int)(Projectile.originalDamage * 0.8f);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // Crit and +50% damage on final segment
            if (Projectile.ai[0] >= Projectile.WhipSettings.Segments - 1 - 0.001f)
            {
                modifiers.FinalDamage *= 1.5f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<SugarCrash>(), 180);

            if (player.whoAmI == Main.myPlayer)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(-2f, 2f), -5f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, vel,
                    ModContent.ProjectileType<SweetGlob>(),
                    damageDone / 2, 0f, player.whoAmI);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            List<Vector2> points = new();
            Projectile.FillWhipControlPoints(Projectile, points);

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            int frameHeight = texture.Height / (Projectile.WhipSettings.Segments + 1);
            int currentFrameY = 0;

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new(0, currentFrameY, texture.Width, frameHeight);
                Vector2 origin = new(texture.Width / 2f, frameHeight / 2f);

                Vector2 diff = points[i + 1] - points[i];
                float rotation = diff.ToRotation();
                Vector2 pos = points[i] - Main.screenPosition;
                Color color = Lighting.GetColor(points[i].ToTileCoordinates());

                sb.Draw(texture, pos, frame, color, rotation, origin, 1f, SpriteEffects.None, 0f);

                currentFrameY += frameHeight;
            }

            return false;
        }
    }
}

