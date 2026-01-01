using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class BiomeCleanserSpinProjectile : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 64; 
            Projectile.height = 64;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 18000;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.channel)
            {
                Projectile.Kill();
                return;
            }
            float angle = Projectile.ai[0] += MathHelper.TwoPi / 30f;
            float radius = 70f;
            Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
            Projectile.Center = player.MountedCenter + offset;
            Projectile.rotation = angle + MathHelper.PiOver4;

            for (int i = 0; i < 6; i++)
            {
                float k = 1f - i / 6f;
                Vector2 drawPos = Projectile.Center - offset * k;
                Color clr = Color.LimeGreen * (int)(120 * k);
                Dust dust = Dust.NewDustPerfect(drawPos, DustID.TerraBlade, Vector2.Zero, 0, clr, 1.7f);
                dust.noGravity = true;
            }
            if (++Projectile.ai[1] >= 12f)
            {
                Projectile.ai[1] = 0f;
                Vector2 toMouse = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitY);
                int crystal = Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    player.Center + toMouse * 20f,
                    toMouse * 13f,
                    ModContent.ProjectileType<WorldsoulCrystalProjectile>(),
                    Projectile.originalDamage,
                    2f,
                    player.whoAmI
                );
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.friendly && target.CanBeChasedBy();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 240);
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(10f, target.Center);
        }
    }
}