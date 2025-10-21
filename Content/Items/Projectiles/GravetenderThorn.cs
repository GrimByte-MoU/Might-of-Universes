namespace MightofUniverses.Content.Items.Projectiles
{
    public class GravetenderThorn : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 2; // pierce 1 enemy (can hit one then expire)
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            // trailing dust
            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 100, default, 0.9f);
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 120); // 2 seconds
            Player owner = Main.player[Projectile.owner];
            if (owner != null && owner.active)
            {
                var reaper = owner.GetModPlayer<ReaperPlayer>();
                reaper?.AddSoulEnergyFromNPC(target, 1f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
}