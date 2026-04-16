using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FireMageMeteor : MoUProjectile
    {
        private const float ExplosionRadius = 160f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Torch, 0f, 0f, 100, default, 1.8f);
                dust.noGravity = true;
                dust.velocity *= 0.4f;
            }

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Smoke, 0f, 0f, 180, default, 1.2f);
                dust.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 1.2f, 0.6f, 0.1f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            Explode();
        }

        private void Explode()
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(8), 16, 16,
                    DustID.Torch, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 100, default, 2.5f);
                dust.noGravity = true;
            }

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(8), 16, 16,
                    DustID.Smoke, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 180, default, 1.5f);
                dust.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 2f, 1f, 0.2f);

            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.friendly || n.dontTakeDamage || n.lifeMax <= 5)
                    continue;

                float dist = Vector2.Distance(n.Center, Projectile.Center);
                if (dist > ExplosionRadius)
                    continue;

                float falloff = 1f - (dist / ExplosionRadius);
                int explodeDamage = (int)(Projectile.damage * falloff);

                NPC.HitInfo hit = new NPC.HitInfo
                {
                    Damage = explodeDamage,
                    Knockback = 6f * falloff,
                    HitDirection = n.Center.X > Projectile.Center.X ? 1 : -1,
                    DamageType = DamageClass.Summon
                };

                n.StrikeNPC(hit);
                n.AddBuff(ModContent.BuffType<CoreHeat>(), 180);
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = texture.Size() * 0.5f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Color trailColor = new Color(255, 120, 30) * alpha * 0.6f;
                Main.EntitySpriteDraw(texture, trailPos, null, trailColor, Projectile.rotation,
                    origin, Projectile.scale * (1f - i * 0.07f), SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null,
                lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}