using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WorldwalkerChilly : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 200;
        }

        public override void OnKill(int timeLeft)
        {
            Vector2 spawnPos = Projectile.Center;
            for (int i = 0; i < 10; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                    new Vector2(spawnPos.X + Main.rand.Next(-5, 6), spawnPos.Y - 500),
                    new Vector2(0, 16f),
                    ModContent.ProjectileType<ChillyIcicle>(),
                    (int)(Projectile.damage * 0.5f),
                    Projectile.knockBack, Projectile.owner);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }

    public class ChillyIcicle : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }
}
