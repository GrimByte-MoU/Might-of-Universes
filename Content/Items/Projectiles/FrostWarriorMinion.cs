namespace MightofUniverses.Content.Items.Projectiles
{
    public class FrostWarriorMinion : MoUProjectile
    {
        private const float SlashRate = 30f;
        private const float BattleCryRate = 180f;
        private const float DetectRange = 320f;
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

        public override bool MinionContactDamage() => true;

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!owner.active || owner.dead || !owner.HasBuff(ModContent.BuffType<FrostWarriorBuff>()))
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

            bool enraged = (float)owner.statLife / owner.statLifeMax2 < 0.5f;
            float speedMult = enraged ? 1.5f : 1f;
            bool hasFullParty = owner.HasBuff(ModContent.BuffType<FullPartyBuff>());

            if (hasFullParty)
                owner.AddBuff(ModContent.BuffType<RalliedBuff>(), 2);

            NPC nearestEnemy = null;
            float nearestDist = float.MaxValue;
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.friendly || n.lifeMax <= 5)
                    continue;

                float dist = Vector2.Distance(owner.Center, n.Center);
                bool grounded = !n.noGravity;

                float effectiveDist = grounded ? dist * 0.5f : dist;

                if (effectiveDist < nearestDist)
                {
                    nearestDist = dist;
                    nearestEnemy = n;
                }
            }

            if (nearestEnemy != null && nearestDist < DetectRange)
            {
                Vector2 toEnemy = nearestEnemy.Center - Projectile.Center;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity,
                    toEnemy.SafeNormalize(Vector2.Zero) * MoveSpeed * speedMult, 0.12f);
                Projectile.spriteDirection = toEnemy.X > 0 ? 1 : -1;

                Projectile.localAI[0]++;
                float slashRate = enraged ? SlashRate * 0.5f : SlashRate;

                if (Projectile.localAI[0] >= slashRate)
                {
                    Projectile.localAI[0] = 0;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int slashDamage = (int)(Projectile.damage * (hasFullParty ? 2f : 1f));
                        nearestEnemy.AddBuff(ModContent.BuffType<SheerCold>(), 120);

                        NPC.HitInfo hit = new NPC.HitInfo
                        {
                            Damage = slashDamage,
                            Knockback = 4f,
                            HitDirection = nearestEnemy.Center.X > Projectile.Center.X ? 1 : -1,
                            DamageType = DamageClass.Summon
                        };
                        nearestEnemy.StrikeNPC(hit);

                        for (int i = 0; i < 8; i++)
                            Dust.NewDustDirect(nearestEnemy.position, nearestEnemy.width, nearestEnemy.height,
                                DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 1.5f).noGravity = true;
                    }
                }
            }
            else
            {
                Vector2 targetPos = owner.Center + new Vector2(owner.direction * 112f, 10f);
                Vector2 toTarget = targetPos - Projectile.Center;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity,
                    toTarget.SafeNormalize(Vector2.Zero) * MoveSpeed * speedMult, 0.1f);
                Projectile.spriteDirection = owner.direction;

                Projectile.localAI[1]++;
                if (Projectile.localAI[1] >= BattleCryRate)
                {
                    Projectile.localAI[1] = 0;

                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player p = Main.player[i];
                        if (!p.active || p.dead)
                            continue;

                        if (Vector2.Distance(p.Center, Projectile.Center) < 160f)
                        {
                            if (hasFullParty)
                                p.AddBuff(ModContent.BuffType<BattleCryBuff>(), 120);
                            else
                                p.AddBuff(ModContent.BuffType<RalliedBuff>(), 180);
                        }
                    }

                    for (int i = 0; i < 20; i++)
                        Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                            DustID.IceTorch, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f),
                            100, Color.LightCyan, 2f).noGravity = true;
                }
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.8f, 1.0f);
        }
    }
}