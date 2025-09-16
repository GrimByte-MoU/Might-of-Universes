using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using System.Linq;

namespace MightofUniverses.Common.GlobalNPCs
{

    public class BossProjectileBuffs : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private static (bool classic, bool expert, bool master) Diff()
        {
            bool master = Main.masterMode;
            bool expert = Main.expertMode;
            bool classic = !(expert || master);
            return (classic, expert, master);
        }
        private static int Sec(float s) => (int)(s * 60f);

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            var (classic, expert, master) = Diff();

            // Queen Bee: stingers travel 20%(25%)[30]% faster
            if (projectile.hostile && projectile.type == ProjectileID.Stinger && NPC.AnyNPCs(NPCID.QueenBee))
            {
                float mul = classic ? 1.20f : expert ? 1.25f : 1.30f;
                projectile.velocity *= mul;
            }

            // Wall of Flesh: per 9% missing HP, laser damage +10%. We apply this by scaling projectile.damage at spawn.
            if (projectile.hostile && projectile.type == ProjectileID.EyeLaser &&
                (NPC.AnyNPCs(NPCID.WallofFlesh) || NPC.AnyNPCs(NPCID.WallofFleshEye)))
            {
                int idx = NPC.FindFirstNPC(NPCID.WallofFlesh);
                if (idx >= 0)
                {
                    NPC wof = Main.npc[idx];
                    float missing = 1f - (float)wof.life / wof.lifeMax;
                    int steps = (int)System.Math.Floor(missing / 0.09f);
                    float dmgMul = 1f + steps * 0.10f;
                    projectile.damage = (int)System.Math.Round(projectile.damage * dmgMul);
                }
            }

            // Skeletron Prime Cannon: approximate "fires 15% more often" by making rockets travel 20% faster in Expert/Master (keeps pressure similar).
            if ((expert || master) && projectile.hostile &&
                (projectile.type == ProjectileID.RocketSkeleton || projectile.type == ProjectileID.BombSkeletronPrime || projectile.type == ProjectileID.GrenadeI) &&
                NPC.AnyNPCs(NPCID.SkeletronPrime))
            {
                projectile.velocity *= 1.20f;
            }

            // Golem lasers travel 15% faster
            if (projectile.hostile && (projectile.type == ProjectileID.EyeBeam || projectile.type == ProjectileID.PinkLaser) &&
                (NPC.AnyNPCs(NPCID.Golem) || NPC.AnyNPCs(NPCID.GolemHead) || NPC.AnyNPCs(NPCID.GolemFistLeft) || NPC.AnyNPCs(NPCID.GolemFistRight)))
            {
                projectile.velocity *= 1.15f;
            }

            // Lunatic Cultist: Lightning Orb - spawn an additional orb "below" player (approximation).
            if (projectile.hostile && projectile.type == ProjectileID.CultistBossLightningOrb && NPC.AnyNPCs(NPCID.CultistBoss))
            {
                // Spawn second orb positioned below the closest player to mimic "parallel below" behavior.
                int pIndex = Player.FindClosest(projectile.Center, 1, 1);
                Player p = Main.player[pIndex];
                Vector2 spawnPos = new Vector2(p.Center.X, p.Center.Y + 200f);
                Projectile.NewProjectile(source, spawnPos, projectile.velocity, ProjectileID.CultistBossLightningOrb, projectile.damage, projectile.knockBack, projectile.owner);
            }

            // Lunatic Cultist: Ice Mist - one extra in Expert, two extra in Master, ~0.1s stagger approximated via extraUpdates/small offset.
            if (projectile.hostile && projectile.type == ProjectileID.CultistBossIceMist && NPC.AnyNPCs(NPCID.CultistBoss))
            {
                int copies = Main.masterMode ? 2 : (Main.expertMode ? 1 : 0);
                for (int i = 0; i < copies; i++)
                {
                    Vector2 spawnPos = projectile.Center + projectile.velocity.SafeNormalize(Vector2.UnitY) * (8f + 6f * i);
                    int id = Projectile.NewProjectile(source, spawnPos, projectile.velocity * 0.98f, ProjectileID.CultistBossIceMist, projectile.damage, projectile.knockBack, projectile.owner);
                    if (id >= 0 && Main.projectile.IndexInRange(id))
                    {
                        Main.projectile[id].extraUpdates = 1; // slight temporal smear
                    }
                }
            }
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            var (classic, expert, master) = Diff();

            // Queen Bee: Stingers inflict Acid Venom in Expert (0.5s) and Master (1.0s)
            if (projectile.hostile && projectile.type == ProjectileID.Stinger && NPC.AnyNPCs(NPCID.QueenBee))
            {
                if (expert) target.AddBuff(BuffID.Venom, Sec(0.5f));
                if (master) target.AddBuff(BuffID.Venom, Sec(1f));
            }

            // Destroyer: body/head lasers Electrified 3/4.5/6 s
            if (projectile.hostile &&
                (NPC.AnyNPCs(NPCID.TheDestroyer) || NPC.AnyNPCs(NPCID.TheDestroyer) || NPC.AnyNPCs(NPCID.TheDestroyerBody)))
            {
                if (projectile.type == ProjectileID.DeathLaser || projectile.type == ProjectileID.LaserMachinegunLaser)
                {
                    target.AddBuff(BuffID.Electrified, Sec(classic ? 3f : expert ? 4.5f : 6f));
                }
            }

            // Destroyer probes' lasers: 1/1.5/2 s Electrified
            if (projectile.hostile && Main.npc.Any(n => n.active && n.type == NPCID.Probe))
            {
                if (projectile.type == ProjectileID.DeathLaser || projectile.type == ProjectileID.LaserMachinegunLaser)
                {
                    target.AddBuff(BuffID.Electrified, Sec(classic ? 1f : expert ? 1.5f : 2f));
                }
            }

            // Spazmatism projectiles: Cursed Inferno 2/3/4 s
            if (projectile.hostile && projectile.type == ProjectileID.CursedFlameHostile && NPC.AnyNPCs(NPCID.Spazmatism))
            {
                target.AddBuff(BuffID.CursedInferno, Sec(classic ? 2f : expert ? 3f : 4f));
            }

            // Retinazer lasers: ignore 10 defense, apply Weaken + Broken Armor 2/3/4 s
            if (projectile.hostile && (projectile.type == ProjectileID.EyeLaser || projectile.type == ProjectileID.PinkLaser) && NPC.AnyNPCs(NPCID.Retinazer))
            {
                modifiers.ArmorPenetration += 10;
                target.AddBuff(BuffID.Weak, Sec(classic ? 2f : expert ? 3f : 4f));
                target.AddBuff(BuffID.BrokenArmor, Sec(classic ? 2f : expert ? 3f : 4f));
            }

            // Skeletron Prime: Cannon inflicts Broken Armor 3/4.5/6 s; Laser inflicts Demonfire 2/3/4 s
            if (projectile.hostile && NPC.AnyNPCs(NPCID.SkeletronPrime))
            {
                if (projectile.type == ProjectileID.RocketSkeleton || projectile.type == ProjectileID.BombSkeletronPrime || projectile.type == ProjectileID.GrenadeI)
                {
                    target.AddBuff(BuffID.BrokenArmor, Sec(classic ? 3f : expert ? 4.5f : 6f));
                }
                if (projectile.type == ProjectileID.PinkLaser)
                {
                    target.AddBuff(ModContent.BuffType<Demonfire>(), Sec(classic ? 2f : expert ? 3f : 4f));
                }
            }

            // Wall of Flesh: lasers inflict Demonfire 0.5s (Expert) / 1.0s (Master)
            if (projectile.hostile && projectile.type == ProjectileID.EyeLaser &&
                (NPC.AnyNPCs(NPCID.WallofFlesh) || NPC.AnyNPCs(NPCID.WallofFleshEye)))
            {
                if (expert) target.AddBuff(ModContent.BuffType<Demonfire>(), Sec(0.5f));
                if (master) target.AddBuff(ModContent.BuffType<Demonfire>(), Sec(1f));
            }

            // Queen Slime: all attacks inflict Rebuking Light 1/1.5/2 s
            if (projectile.hostile && NPC.AnyNPCs(NPCID.QueenSlimeBoss))
            {
                target.AddBuff(ModContent.BuffType<RebukingLight>(), Sec(classic ? 1f : expert ? 1.5f : 2f));
            }

            // Lunatic Cultist:
            if (projectile.hostile && NPC.AnyNPCs(NPCID.CultistBoss))
            {
                // Fireball: armor pen 20/25/30 + Demonfire 2/3/4 s
                if (projectile.type == ProjectileID.CultistBossFireBall)
                {
                    modifiers.ArmorPenetration += (classic ? 20 : expert ? 25 : 30);
                    target.AddBuff(ModContent.BuffType<Demonfire>(), Sec(classic ? 2f : expert ? 3f : 4f));
                }
                // Lightning Orb and Bolt: Electrified 5/7.5/10 s
                else if (projectile.type == ProjectileID.CultistBossLightningOrb)
                {
                    target.AddBuff(BuffID.Electrified, Sec(classic ? 5f : expert ? 7.5f : 10f));
                }
                else if (projectile.type == ProjectileID.CultistBossLightningOrbArc)
                {
                    target.AddBuff(BuffID.Electrified, Sec(classic ? 5f : expert ? 7.5f : 10f));
                }
                // Ice Mist: Frostburn 2/3/4 s
                else if (projectile.type == ProjectileID.CultistBossIceMist)
                {
                    target.AddBuff(BuffID.Frostburn, Sec(classic ? 2f : expert ? 3f : 4f));
                }
                
                // Shadow Fireball: Demonfire + Goblin's Curse 1/1.5/2 s
                else if (projectile.type == ProjectileID.ShadowFlame)
                {
                    target.AddBuff(ModContent.BuffType<Demonfire>(), Sec(classic ? 1f : expert ? 1.5f : 2f));
                    target.AddBuff(ModContent.BuffType<GoblinsCurse>(), Sec(classic ? 1f : expert ? 1.5f : 2f));
                }
            }

            // Moon Lord: Lunar Reap durations per source
            if (projectile.hostile &&
                (NPC.AnyNPCs(NPCID.MoonLordCore) || NPC.AnyNPCs(NPCID.MoonLordHead) || NPC.AnyNPCs(NPCID.MoonLordHand)))
            {
                if (projectile.type == ProjectileID.PhantasmalDeathray)
                {
                    target.AddBuff(ModContent.BuffType<LunarReap>(), Sec(classic ? 3f : expert ? 4.5f : 6f));
                }
                else if (projectile.type == ProjectileID.PhantasmalBolt)
                {
                    target.AddBuff(ModContent.BuffType<LunarReap>(), Sec(classic ? 1f : expert ? 1.5f : 2f));
                }
                else if (projectile.type == ProjectileID.MoonBoulder || projectile.type == ProjectileID.PhantasmalSphere || projectile.type == ProjectileID.PhantasmalEye)
                {
                    target.AddBuff(ModContent.BuffType<LunarReap>(), Sec(classic ? 2f : expert ? 3f : 4f));
                }
            }
        }
    }
}