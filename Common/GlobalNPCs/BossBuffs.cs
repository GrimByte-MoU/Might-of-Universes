using System;
using System. Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria. ID;
using Terraria. ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalNPCs
{
    // Implements NPC-side parts of the simplified boss buffs: 
    // - Stat scaling in SetDefaults (HP/def/damage).
    // - Movement/phase logic and simple periodic spawns in AI where specified.
    // - Contact-based debuffs and defense-ignore in ModifyHitPlayer.
    //
    // Projectile-specific changes (debuffs, laser scaling) are in BossProjectileBuffs.cs.
    public class BossBuffs : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        // Local state
        private bool baseDefCaptured = false;
        private int baseDef = 0;

        private static (bool classic, bool expert, bool master) Diff()
        {
            bool master = Main.masterMode;
            bool expert = Main.expertMode;
            bool classic = !(expert || master);
            return (classic, expert, master);
        }

        private static int Sec(float s) => (int)(s * 60f);

        private void SnapshotBaseDef(NPC npc)
        {
            if (!baseDefCaptured)
            {
                baseDefCaptured = true;
                baseDef = npc.defense;
            }
        }

        public override void SetDefaults(NPC npc)
        {
            void Scale(float hpMul, int defAdd, float dmgMul)
            {
                npc.lifeMax = (int)Math.Round(npc.lifeMax * hpMul);
                npc.defense += defAdd;
                npc. damage = (int)Math.Round(npc.damage * dmgMul);
            }

            var (classic, expert, master) = Diff();

            switch (npc.type)
            {
                // King Slime:  +2 def, +20% HP, +10% dmg
                case NPCID.KingSlime:
                    Scale(1.20f, 2, 1.10f);
                    break;

                // Eye of Cthulhu: +10% HP, +10% dmg
                case NPCID.EyeofCthulhu:
                    Scale(1.10f, 0, 1.10f);
                    break;

                // Eater of Worlds segments: 
                case NPCID.EaterofWorldsHead:
                    npc.defense += 3;
                    npc.damage = (int)Math.Round(npc.damage * 2.0f); // head damage doubled
                    break;
                case NPCID.EaterofWorldsBody:
                    npc. defense += 3;
                    break;
                case NPCID.EaterofWorldsTail:
                    npc.defense += 3;
                    npc.damage = (int)Math.Round(npc.damage * 0.75f); // tail -25% dmg
                    break;

                // Brain of Cthulhu:  movement + spawns handled in AI
                case NPCID.BrainofCthulhu:
                    break;

                // Queen Bee: stinger speed + venoms done via projectiles
                case NPCID.QueenBee:
                    break;

                // Skeletron:  hands +25% HP, +10% dmg; head spin scaling handled in AI
                case NPCID.SkeletronHand:
                    Scale(1.25f, 0, 1.10f);
                    break;
                case NPCID. SkeletronHead:
                    break;

                // Wall of Flesh: laser damage scaling + Demonfire handled via projectiles
                case NPCID.WallofFlesh:
                case NPCID.WallofFleshEye:
                    break;

                // Queen Slime: phase 2 +10 def, +20% dmg (handled in AI); Rebuking Light via projectiles/contact
                case NPCID.QueenSlimeBoss:
                    break;

                // Destroyer segments
                case NPCID.TheDestroyer:
                    npc.damage = (int)Math.Round(npc.damage * 1.25f);
                    break;
                case NPCID.TheDestroyerBody:
                    // Body segments keep normal stats
                    break;
                case NPCID.TheDestroyerTail:
                    npc.damage = (int)Math.Round(npc.damage * 0.75f);
                    break;

                // Twins
                case NPCID. Spazmatism:
                    npc.defense += 10; // +10 defense
                    break;
                case NPCID.Retinazer:
                    npc.damage = (int)Math.Round(npc.damage * 1.10f); // +10% damage
                    break;

                // Skeletron Prime set: 
                case NPCID. PrimeSaw:
                    npc.damage = (int)Math.Round(npc.damage * 1.15f); // +15% for Saw
                    break;
                case NPCID.PrimeVice:
                case NPCID.PrimeCannon:
                case NPCID. SkeletronPrime:
                case NPCID.PrimeLaser:
                    break;

                // Plantera:  +15% HP and dmg
                case NPCID.Plantera:
                    Scale(1.15f, 0, 1.15f);
                    break;

                case NPCID.Golem:
                case NPCID.GolemHead:
                case NPCID.GolemFistLeft:
                case NPCID.GolemFistRight:
                    Scale(1.30f, 0, 1.30f);
                    break;

                // Duke Fishron: +20 def, +20% HP, +10% dmg
                case NPCID.DukeFishron:
                    Scale(1.20f, 20, 1.10f);
                    break;

                // Lunatic Cultist: double HP (rest via projectiles)
                case NPCID.CultistBoss:
                    Scale(2.0f, 0, 1.0f);
                    break;

                // Moon Lord: debuffs via projectiles/contact below
                case NPCID. MoonLordCore:
                case NPCID.MoonLordHead:
                case NPCID.MoonLordHand:
                    break;
            }
        }

        public override void AI(NPC npc)
        {
            var (classic, expert, master) = Diff();

            // Brain of Cthulhu: Phase 2: +4 def and spawn a Creeper every 3/2.5/2 s
            if (npc.type == NPCID.BrainofCthulhu)
            {
                bool creepersRemain = Main.npc.Any(n => n.active && n.type == NPCID.Creeper);
                bool phase2 = ! creepersRemain;

                SnapshotBaseDef(npc);
                if (phase2)
                {
                    npc.defense = baseDef + 4;

                    ref float timer = ref npc.localAI[0];
                    timer++;
                    float interval = classic ? 180f : expert ?  150f : 120f; // 3.0 / 2.5 / 2.0 s
                    if (timer >= interval)
                    {
                        timer = 0f;
                        // Reasonable cap
                        int existing = Main.npc.Count(n => n. active && n.type == NPCID.Creeper);
                        if (existing < 12)
                        {
                            // Spawn in valid location (not inside blocks)
                            Vector2 spawnPos = npc.Center + Main.rand.NextVector2Circular(120, 120);
                            Point tilePos = spawnPos.ToTileCoordinates();
                            
                            if (WorldGen.InWorld(tilePos.X, tilePos.Y) && !WorldGen.SolidTile(tilePos.X, tilePos.Y))
                            {
                                int id = NPC. NewNPC(npc.GetSource_FromAI(), (int)spawnPos.X, (int)spawnPos.Y, NPCID.Creeper);
                                if (id >= 0 && Main.netMode != NetmodeID.SinglePlayer)
                                    NetMessage. SendData(MessageID.SyncNPC, -1, -1, null, id);
                            }
                        }
                    }
                }
                else
                {
                    npc. defense = baseDef;
                }
            }

            // Skeletron Head: per 9% missing, +5/10/15 def while spinning
            if (npc.type == NPCID. SkeletronHead)
            {
                SnapshotBaseDef(npc);
                bool spinning = npc.ai[1] == 3f; // vanilla spin indicator

                if (spinning)
                {
                    npc.defense = baseDef + (classic ?  5 : expert ? 10 : 15);
                }
                else
                {
                    npc.defense = baseDef;
                }
            }

            // Queen Slime:  Phase 2 -> +10 def and +20% damage
            if (npc.type == NPCID.QueenSlimeBoss)
            {
                SnapshotBaseDef(npc);
                bool phase2 = npc.life < npc.lifeMax * 0.5f;
                if (phase2)
                {
                    npc.defense = baseDef + 10;
                    if (!npc.localAI[3].Equals(1f))
                    {
                        npc.damage = (int)Math.Round(npc.damage * 1.20f);
                        npc.localAI[3] = 1f; // mark applied
                    }
                }
                else
                {
                    npc.defense = baseDef;
                    // Revert damage boost if healed back to phase 1
                    if (npc. localAI[3]. Equals(1f))
                    {
                        npc. damage = (int)Math.Round(npc.damage / 1.20f);
                        npc.localAI[3] = 0f;
                    }
                }
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            var (classic, expert, master) = Diff();

            // Eater of Worlds:  Corrupted debuff on contact (Head 3/4/5 s, Body 1/2/3 s, Tail none)
            if (npc. type == NPCID.EaterofWorldsHead)
            {
                target.AddBuff(ModContent. BuffType<Corrupted>(), Sec(classic ? 3f : expert ? 4f : 5f));
            }
            else if (npc.type == NPCID.EaterofWorldsBody)
            {
                target.AddBuff(ModContent. BuffType<Corrupted>(), Sec(classic ? 1f : expert ? 2f :  3f));
            }

            if (npc.type == NPCID.TheDestroyer)
            {
                modifiers.ArmorPenetration += 30;
            }

            // Spazmatism:  Bleeding on contact 2/3/4 s
            if (npc.type == NPCID.Spazmatism)
            {
                target. AddBuff(BuffID.Bleeding, Sec(classic ? 2f : expert ?  3f : 4f));
            }

            // Retinazer: 10 defense ignored on contact as well
            if (npc.type == NPCID.Retinazer)
            {
                modifiers.ArmorPenetration += 10;
            }

            // Prime Vice: ignore 15/20/25 defense on contact
            if (npc.type == NPCID. PrimeVice)
            {
                modifiers.ArmorPenetration += classic ? 15 : expert ? 20 : 25;
            }

            // Prime Saw: apply Bleeding 2/3/4 s on contact
            if (npc. type == NPCID.PrimeSaw)
            {
                target.AddBuff(BuffID.Bleeding, Sec(classic ? 2f : expert ?  3f : 4f));
            }

            // Queen Slime: contact applies Rebuking Light 1/1.5/2 s
            if (npc.type == NPCID.QueenSlimeBoss)
            {
                target.AddBuff(ModContent.BuffType<RebukingLight>(), Sec(classic ? 1f : expert ? 1.5f : 2f));
            }

            // Moon Lord hand contact: Lunar Reap 1/1.5/2 s
            if (npc.type == NPCID.MoonLordHand)
            {
                target.AddBuff(ModContent.BuffType<LunarReap>(), Sec(classic ? 1f : expert ? 1.5f : 2f));
            }
        }
    }
}