namespace MightofUniverses.Content.Items.Projectiles
{
    public class WindRangerArrow : MoUProjectile
    {
        private bool IsPierce => Projectile.ai[0] == 1f;

        public override void SafeSetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (IsPierce && Projectile.penetrate == 1)
                Projectile.penetrate = 5;

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(4))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Electric, 0f, 0f, 100, default, 0.8f).noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DeltaShock>(), 120);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
                Dust.NewDustDirect(Projectile.Center - new Vector2(4), 8, 8,
                    DustID.Electric, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f),
                    100, default, 1.2f).noGravity = true;
        }
    }
}