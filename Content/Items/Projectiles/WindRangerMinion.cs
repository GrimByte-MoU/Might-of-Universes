namespace MightofUniverses.Content.Items.Projectiles
{
    public class WindRangerMinion : MoUProjectile
    {
        private const float BaseFireRate = 20f;
        private const int EnemyThreshold = 10;
        private const float MoveSpeed = 6f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
            Main.projPet[Type] = true;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 3f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.netImportant = true;
        }

        public override bool MinionContactDamage() => false;

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!owner.active || owner.dead || !owner.HasBuff(ModContent.BuffType<WindRangerBuff>()))
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            int count = 0;
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.owner == Projectile.owner && p.type == Type)
                    count++;
            }
            if (count > 1)
            {
                Projectile.Kill();
                return;
            }

            Vector2 targetPos = owner.Center + new Vector2(-owner.direction * 112f, 0f);
            Vector2 toTarget = targetPos - Projectile.Center;

            if (toTarget.Length() > 10f)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity,
                    toTarget.SafeNormalize(Vector2.Zero) * MoveSpeed, 0.1f);
            else
                Projectile.velocity *= 0.8f;

            Projectile.spriteDirection = owner.direction;

            if (Main.rand.NextBool(6))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Electric, 0f, 0f, 100, default, 1.0f).noGravity = true;

            Lighting.AddLight(Projectile.Center, 0.2f, 0.6f, 1.0f);

            bool manyEnemies = false;
            int enemyCount = 0;
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (!n.friendly && n.lifeMax > 5)
                    enemyCount++;
            }
            manyEnemies = enemyCount >= EnemyThreshold;

            bool hasFullParty = owner.HasBuff(ModContent.BuffType<FullPartyBuff>());
            float fireRate = manyEnemies ? BaseFireRate * 0.5f : BaseFireRate;

            Projectile.localAI[0]++;

            if (Projectile.localAI[0] < fireRate)
                return;

            Projectile.localAI[0] = 0;

            NPC nearestEnemy = null;
            float nearestDist = 1000f;
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.friendly || n.lifeMax <= 5)
                    continue;

                float dist = Vector2.Distance(Projectile.Center, n.Center);
                bool flying = n.noGravity;

                float effectiveDist = flying ? dist * 0.5f : dist;

                if (effectiveDist < nearestDist)
                {
                    nearestDist = dist;
                    nearestEnemy = n;
                }
            }

            if (nearestEnemy == null || Main.netMode == NetmodeID.MultiplayerClient)
                return;

            int arrowDamage = (int)(Projectile.damage * 0.80f * (hasFullParty ? 2f : 1f));
            bool pierce = Main.rand.NextBool();

            Vector2 vel = (nearestEnemy.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 14f;

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel,
                ModContent.ProjectileType<WindRangerArrow>(), arrowDamage, 2f, Projectile.owner,
                pierce ? 1f : 0f);
        }
    }
}