using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna. Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MiniWorldsoulBolt : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile. penetrate = 1;
            Projectile. timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.Cyan, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.2f;
            }

            Lighting.AddLight(Projectile.Center, 0.2f, 0.6f, 0.6f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Cyan;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 60);

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TerraBlade, 0, 0, 100, Color.Cyan, 1.5f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color.Cyan, 1.0f);
                dust.noGravity = true;
            }
            return true;
        }
    }
}