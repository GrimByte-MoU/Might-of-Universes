using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class Worldbolt : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 0.6f, 0.9f, 0.6f);

            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Enchanted_Gold, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int dur = 420;
            target.AddBuff(BuffID.Ichor, dur);
            target.AddBuff(BuffID.Venom, dur);
            target.AddBuff(BuffID.CursedInferno, dur);
            target.AddBuff(BuffID.Frostburn, dur);
            target.AddBuff(BuffID.OnFire3, dur);
            target.AddBuff(BuffID.Electrified, dur);
        }
    }
}
