using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class BluePrismaticProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 400;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.damage = (int)(Projectile.damage * 0.75f);
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.1f, 0.1f, 1f);
            //Projectile.rotation += 0.4f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch, 0f, 0f, 0, default, 1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
        }
    }
}
