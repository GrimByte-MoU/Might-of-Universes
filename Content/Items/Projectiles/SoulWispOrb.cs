namespace MightofUniverses.Content.Items.Projectiles
{
    public class SoulWispOrb : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Vector2 toPlayer = owner.Center - Projectile.Center;
            float dist = toPlayer.Length();
            if (dist > 6f)
            {
                toPlayer.Normalize();
                Projectile.velocity = (Projectile.velocity * 14f + toPlayer * 6f) / 15f;
            }
            else
            {
                // Collect
                var reaper = owner.GetModPlayer<ReaperPlayer>();
                reaper.AddSoulEnergy(0.2f, owner.Center);
                int heal = Main.rand.Next(1, 3);
                owner.statLife = System.Math.Min(owner.statLifeMax2, owner.statLife + heal);
                owner.HealEffect(heal, true);
                Projectile.Kill();
            }

            // Cosmetic dust
            if (Main.rand.NextBool(5))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch, 0f, 0f, 150, Color.Cyan, 0.9f);
                Main.dust[d].noGravity = true;
            }
        }
    }
}