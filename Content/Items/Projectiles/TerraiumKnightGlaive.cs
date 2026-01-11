using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TerraiumKnightGlaive : MoUProjectile
    {
        private float orbitAngle;
        private const float OrbitRadius = 112f;
        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (! player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            var armorPlayer = player.GetModPlayer<TerraiumArmorPlayer>();
            if (! armorPlayer.knightSetBonus)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 300;

            int glaiveIndex = (int)Projectile.ai[0];
            float baseAngle = glaiveIndex / 3f * MathHelper. TwoPi;
            orbitAngle = baseAngle + (Main.GameUpdateCount * 0.06f);

            Vector2 offset = new Vector2(
                (float)Math.Cos(orbitAngle) * OrbitRadius,
                (float)Math.Sin(orbitAngle) * OrbitRadius
            );

            Projectile.Center = player.Center + offset;
            Projectile.rotation += 0.2f;
            
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile. Center, 0.4f, 0.9f, 0.4f);
        }

        public override Color?  GetAlpha(Color lightColor)
        {
            return Color. LimeGreen;
        }

        public override void OnHitNPC(NPC target, NPC. HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent. BuffType<TerrasRend>(), 120);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TerraBlade, 0, 0, 100, Color.LimeGreen, 2.0f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
            }
        }
    }
}