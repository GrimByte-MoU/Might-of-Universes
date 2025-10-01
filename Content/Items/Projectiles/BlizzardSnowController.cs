using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class BlizzardSnowController : ModProjectile
    {
        // ai[0] = total snowflakes to spawn
        // ai[1] = total duration ticks
        // localAI[0] = spawned so far
        // localAI[1] = tick counter

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 600; // safety
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.hide = true;
        }

        public override void AI()
        {
            int total = (int)Projectile.ai[0];
            int duration = (int)Projectile.ai[1];
            if (duration <= 0) duration = 240;

            int spawned = (int)Projectile.localAI[0];
            int ticks = (int)Projectile.localAI[1];

            if (spawned >= total)
            {
                Projectile.Kill();
                return;
            }

            // Evenly distribute: expected spawn index for this tick
            // fraction = ticks / duration -> expected total = total * fraction
            float expected = total * (ticks / (float)duration);

            // Spawn until we meet expected (handles fractional accumulation smoothly)
            while (spawned < total && spawned < expected)
            {
                SpawnOne();
                spawned++;
            }

            Projectile.localAI[0] = spawned;
            Projectile.localAI[1] = ticks + 1;

            if (ticks >= duration + 5)
                Projectile.Kill();
        }

        private void SpawnOne()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active) return;

            IEntitySource src = Projectile.GetSource_FromThis();
            int damage = Projectile.damage;
            float kb = Projectile.knockBack;

            Vector2 pos = new Vector2(
                owner.Center.X + Main.rand.NextFloat(-600f, 600f),
                owner.Center.Y - Main.rand.NextFloat(600f, 800f)
            );
            Vector2 vel = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(12f, 16f));

            Projectile.NewProjectile(
                src,
                pos,
                vel,
                ModContent.ProjectileType<BlizzardSnowflakeProjectile>(),
                damage,
                kb,
                owner.whoAmI
            );
        }
    }
}