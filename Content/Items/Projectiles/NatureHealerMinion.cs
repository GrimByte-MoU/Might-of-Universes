namespace MightofUniverses.Content.Items.Projectiles
{
    public class NatureHealerMinion : MoUProjectile
    {
        private const float AttackRate = 20f;
        private const float HealRate = 60f;
        private const float FlySpeed = 7f;

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

            if (!owner.active || owner.dead || !owner.HasBuff(ModContent.BuffType<NatureHealerBuff>()))
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

            Vector2 targetPos = owner.Center + new Vector2(-owner.direction * 80f, -140f);
            Vector2 toTarget = targetPos - Projectile.Center;

            if (toTarget.Length() > 8f)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity,
                    toTarget.SafeNormalize(Vector2.Zero) * FlySpeed, 0.1f);
            else
                Projectile.velocity *= 0.8f;

            Projectile.spriteDirection = owner.direction;

            if (Main.rand.NextBool(6))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.JunglePlants, 0f, 0f, 100, default, 1.2f).noGravity = true;

            Lighting.AddLight(Projectile.Center, 0.2f, 1.0f, 0.3f);

            bool hasFullParty = owner.HasBuff(ModContent.BuffType<FullPartyBuff>());
            bool playerLow = (float)owner.statLife / owner.statLifeMax2 < 0.5f;

            Projectile.localAI[0]++;

            if (!playerLow)
            {
                if (Projectile.localAI[0] < AttackRate)
                    return;

                Projectile.localAI[0] = 0;

                NPC nearestEnemy = null;
                float nearestDist = 800f;
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.friendly || n.lifeMax <= 5)
                        continue;

                    float dist = Vector2.Distance(Projectile.Center, n.Center);
                    if (dist < nearestDist)
                    {
                        nearestDist = dist;
                        nearestEnemy = n;
                    }
                }

                if (nearestEnemy == null || Main.netMode == NetmodeID.MultiplayerClient)
                    return;

                int attackDamage = (int)(Projectile.damage * 0.75f * (hasFullParty ? 2f : 1f));
                Vector2 vel = (nearestEnemy.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel,
                    ModContent.ProjectileType<NatureOrbProjectile>(), attackDamage, 1f, Projectile.owner);
            }
            else
            {
                if (Projectile.localAI[0] < HealRate)
                    return;

                Projectile.localAI[0] = 0;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    return;

                Vector2 vel = (owner.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 8f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel,
                    ModContent.ProjectileType<HealOrbProjectile>(), 0, 0f, Projectile.owner);
            }
        }
    }
}