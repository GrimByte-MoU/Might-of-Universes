namespace MightofUniverses.Content.Items.Projectiles
{
    public class HealOrbProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Vector2 toPlayer = owner.Center - Projectile.Center;

            if (toPlayer.Length() < 20f)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    owner.statLife = System.Math.Min(owner.statLife + 5, owner.statLifeMax2);
                    owner.HealEffect(5);
                }
                Projectile.Kill();
                return;
            }

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, toPlayer.SafeNormalize(Vector2.Zero) * 10f, 0.15f);
            Projectile.rotation += 0.2f;

            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                DustID.JunglePlants, 0f, 0f, 100, default, 1.0f).noGravity = true;
        }
    }
}