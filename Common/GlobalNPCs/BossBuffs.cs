using System;
using System. Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria. ID;
using Terraria. ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class BossBuffs : GlobalNPC
    {
        public override bool InstancePerEntity => true;
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
                case NPCID.KingSlime:
                    Scale(1.20f, 2, 1.10f);
                    break;

                case NPCID.EyeofCthulhu:
                    Scale(1.10f, 0, 1.10f);
                    break;

                case NPCID.EaterofWorldsHead:
                    npc.defense += 3;
                    npc.damage = (int)Math.Round(npc.damage * 2.0f);
                    break;
                case NPCID.EaterofWorldsBody:
                    npc. defense += 3;
                    break;
                case NPCID.EaterofWorldsTail:
                    npc.defense += 3;
                    npc.damage = (int)Math.Round(npc.damage * 0.75f);
                    break;

                case NPCID.BrainofCthulhu:
                    break;

                case NPCID.QueenBee:
                    break;

                case NPCID.SkeletronHand:
                    Scale(1.25f, 0, 1.10f);
                    break;
                case NPCID. SkeletronHead:
                    break;

                case NPCID.WallofFlesh:
                case NPCID.WallofFleshEye:
                    break;

                case NPCID.QueenSlimeBoss:
                    break;

                case NPCID.TheDestroyer:
                    npc.damage = (int)Math.Round(npc.damage * 1.25f);
                    break;
                case NPCID.TheDestroyerBody:
                    break;
                case NPCID.TheDestroyerTail:
                    npc.damage = (int)Math.Round(npc.damage * 0.75f);
                    break;

                case NPCID. Spazmatism:
                    npc.defense += 10;
                    break;
                case NPCID.Retinazer:
                    npc.damage = (int)Math.Round(npc.damage * 1.10f);
                    break;

                case NPCID. PrimeSaw:
                    npc.damage = (int)Math.Round(npc.damage * 1.15f);
                    break;
                case NPCID.PrimeVice:
                case NPCID.PrimeCannon:
                case NPCID. SkeletronPrime:
                case NPCID.PrimeLaser:
                    break;

                case NPCID.Plantera:
                    Scale(1.15f, 0, 1.15f);
                    break;

                case NPCID.Golem:
                case NPCID.GolemHead:
                case NPCID.GolemFistLeft:
                case NPCID.GolemFistRight:
                    Scale(1.30f, 0, 1.30f);
                    break;

                case NPCID.DukeFishron:
                    Scale(1.20f, 20, 1.10f);
                    break;

                case NPCID.CultistBoss:
                    Scale(2.0f, 0, 1.0f);
                    break;

                case NPCID. MoonLordCore:
                case NPCID.MoonLordHead:
                case NPCID.MoonLordHand:
                    break;
            }
        }

        public override void AI(NPC npc)
        {
            var (classic, expert, master) = Diff();

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
                    float interval = classic ? 180f : expert ?  150f : 120f;
                    if (timer >= interval)
                    {
                        timer = 0f;
                        int existing = Main.npc.Count(n => n. active && n.type == NPCID.Creeper);
                        if (existing < 12)
                        {
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
            if (npc.type == NPCID. SkeletronHead)
            {
                SnapshotBaseDef(npc);
                bool spinning = npc.ai[1] == 3f;

                if (spinning)
                {
                    npc.defense = baseDef + (classic ?  5 : expert ? 10 : 15);
                }
                else
                {
                    npc.defense = baseDef;
                }
            }

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
                        npc.localAI[3] = 1f;
                    }
                }
                else
                {
                    npc.defense = baseDef;
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
            
            if (npc.type == NPCID.Spazmatism)
            {
                target.AddBuff(BuffID.Bleeding, Sec(classic ? 2f : expert ? 3f : 4f));
            }

            if (npc.type == NPCID.Retinazer)
            {
                modifiers.ArmorPenetration += 10;
            }

            if (npc.type == NPCID. PrimeVice)
            {
                modifiers.ArmorPenetration += classic ? 15 : expert ? 20 : 25;
            }

            if (npc. type == NPCID.PrimeSaw)
            {
                target.AddBuff(BuffID.Bleeding, Sec(classic ? 2f : expert ?  3f : 4f));
            }

            if (npc.type == NPCID.QueenSlimeBoss)
            {
                target.AddBuff(ModContent.BuffType<RebukingLight>(), Sec(classic ? 1f : expert ? 1.5f : 2f));
            }

            if (npc.type == NPCID.MoonLordHand)
            {
                target.AddBuff(ModContent.BuffType<LunarReap>(), Sec(classic ? 1f : expert ? 1.5f : 2f));
            }
        }
    }
}