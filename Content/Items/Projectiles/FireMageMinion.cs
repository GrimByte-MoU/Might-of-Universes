using System.Linq;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FireMageMinion : MoUProjectile
    {
        private const float AttackRateFew = 15f;
        private const float AttackRateMany = 30f;
        private const int EnemyThreshold = 10;
        private const float FlySpeed = 8f;

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

            if (!owner.active || owner.dead || !owner.HasBuff(ModContent.BuffType<FireMageBuff>()))
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

            Vector2 targetPos = owner.Center + new Vector2(owner.direction * 100f, -140f);
            Vector2 toTarget = targetPos - Projectile.Center;

            if (toTarget.Length() > 8f)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, toTarget.SafeNormalize(Vector2.Zero) * FlySpeed, 0.1f);
            else
                Projectile.velocity *= 0.8f;

            Projectile.spriteDirection = owner.direction;

            if (Main.rand.NextBool(8))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Torch, 0f, 0f, 100, default, 1.2f).noGravity = true;

            Lighting.AddLight(Projectile.Center, 1.0f, 0.5f, 0.1f);

            Projectile.localAI[0]++;

            int enemyCount = Main.npc.Count(n => n.active && !n.friendly && n.lifeMax > 5);
            bool manyEnemies = enemyCount >= EnemyThreshold;
            float cooldown = manyEnemies ? AttackRateMany : AttackRateFew;

            if (Projectile.localAI[0] < cooldown)
                return;

            Projectile.localAI[0] = 0;

            NPC nearestEnemy = null;
            float nearestDist = 800f;
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (!n.friendly && n.lifeMax > 5)
                {
                    float dist = Vector2.Distance(Projectile.Center, n.Center);
                    if (dist < nearestDist)
                    {
                        nearestDist = dist;
                        nearestEnemy = n;
                    }
                }
            }

            if (nearestEnemy == null || Main.netMode == NetmodeID.MultiplayerClient)
                return;

            bool hasFullParty = owner.HasBuff(ModContent.BuffType<FullPartyBuff>());
            int attackDamage = (int)(Projectile.damage * (manyEnemies ? 1.5f : 0.75f) * (hasFullParty ? 2f : 1f));

            if (!manyEnemies)
            {
                Vector2 vel = (nearestEnemy.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 12f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel,
                    ModContent.ProjectileType<FireMageFirebolt>(), attackDamage, 2f, Projectile.owner);
            }
            else
            {
                Vector2 spawnPos = new Vector2(nearestEnemy.Center.X, nearestEnemy.Center.Y - 600f);
                Vector2 vel = (nearestEnemy.Center - spawnPos).SafeNormalize(Vector2.Zero) * 16f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, vel,
                    ModContent.ProjectileType<FireMageMeteor>(), attackDamage, 4f, Projectile.owner);
            }
        }
    }
}