using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Terraria.ModLoader.Utilities;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.NPCs.Bosses.Minions;
using MightofUniverses.Common.Systems;
using MightofUniverses.Content.Items.Consumables.BossBags;

namespace MightofUniverses.Content.NPCs.Bosses
{
    [AutoloadBossHead] // Enables minimap + health bar icon (ensure ObeSadee_Head_Boss.png exists)
    public class ObeSadee : ModNPC
    {
        private enum Phase { Phase1, Phase2 }
        private Phase currentPhase = Phase.Phase1;

        private int jumpCount;
        private int attackCooldown;
        private int animationType = 0; // 0 = Idle, 1 = Jump, 2 = Attack
        private int phaseTimer;
        private bool isExpert => Main.expertMode;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1; // Handled manually
        }

        public override void SetDefaults()
        {
            NPC.width = 150;
            NPC.height = 150;
            NPC.damage = 40;
            NPC.defense = 25;
            NPC.lifeMax = isExpert ? 20000 : 10000;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.aiStyle = -1;
            Music = MusicID.Boss2;
            NPC.npcSlots = 10f;
            NPC.value = Item.buyPrice(0, 10, 0, 0);

            // Boss Zen - prevent other NPCs from spawning
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                NPC.velocity.Y += 0.2f;
                if (NPC.timeLeft > 10)
                    NPC.timeLeft = 10;
                return;
            }

            NPC.TargetClosest();

            if (currentPhase == Phase.Phase1 && NPC.life < NPC.lifeMax * (isExpert ? 0.75f : 0.5f))
            {
                currentPhase = Phase.Phase2;
                jumpCount = 0;
                phaseTimer = 0;
                NPC.netUpdate = true;
            }

            if (isExpert && currentPhase == Phase.Phase2 && Main.GameUpdateCount % 20 == 0)
            {
                Vector2 spawn = player.Center + new Vector2(Main.rand.NextFloat(-400, 400), -600);
                Vector2 velocity = Vector2.UnitY * 12f;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, velocity, ModContent.ProjectileType<ObeSadeeToothProjectile>(), 20, 0f, Main.myPlayer);
            }

            switch (currentPhase)
            {
                case Phase.Phase1: RunPhase1AI(player); break;
                case Phase.Phase2: RunPhase2AI(player); break;
            }
        }

        private void RunPhase1AI(Player player)
        {
            phaseTimer++;
            if (attackCooldown > 0) { attackCooldown--; return; }

            if (jumpCount < 3)
            {
                if (phaseTimer > 45 && NPC.velocity.Y == 0f)
                {
                    JumpTowardPlayer(player, 12f, 5f);
                    jumpCount++;
                    phaseTimer = 0;
                }
            }
            else
            {
                animationType = 2;
                NPC.defense = 40;
                FireTeeth(player, isExpert ? 3 : 1);
                attackCooldown = isExpert ? 60 : 90;
                jumpCount = 0;
                NPC.defense = 25;
            }
        }

        private void RunPhase2AI(Player player)
        {
            phaseTimer++;
            if (attackCooldown > 0) { attackCooldown--; return; }

            if (jumpCount < Main.rand.Next(3, 6))
            {
                if (phaseTimer > (isExpert ? 25 : 40) && NPC.velocity.Y == 0f)
                {
                    bool superJump = isExpert && Main.rand.NextFloat() < 0.2f;
                    if (superJump)
                    {
                        SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                        phaseTimer += 20;
                        JumpTowardPlayer(player, 18f, 8f);
                    }
                    else
                        JumpTowardPlayer(player, 12f, 5f);

                    jumpCount++;
                    phaseTimer = 0;
                }
            }
            else
            {
                animationType = 2;
                jumpCount = 0;
                SelectPhase2Attack(player);
                attackCooldown = isExpert ? 120 : 90;
            }
        }

        private void JumpTowardPlayer(Player player, float yVel, float xVel)
        {
            animationType = 1;
            NPC.velocity.Y = -yVel;
            NPC.velocity.X = (player.Center.X > NPC.Center.X ? 1 : -1) * xVel;
        }

        private void SelectPhase2Attack(Player player)
        {
            switch (Main.rand.Next(3))
            {
                case 0: FireTeeth(player, isExpert ? 9 : 5); break;
                case 1: SpawnToothRain(player); break;
                case 2: SummonHealerMinion(); break;
            }
        }

        private void FireTeeth(Player player, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 velocity = Vector2.Normalize(player.Center - NPC.Center).RotatedByRandom(0.2f) * (6f + i);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<ObeSadeeToothProjectile>(), 20, 0f, Main.myPlayer);
            }
        }

        private void SpawnToothRain(Player player)
        {
            for (int row = 0; row < (isExpert ? 5 : 3); row++)
            {
                for (int i = 0; i < (isExpert ? 19 : 13); i++)
                {
                    Vector2 spawn = new Vector2(player.Center.X - 300 + i * 30, player.Center.Y - 600 + row * 30);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, Vector2.UnitY * 10f, ModContent.ProjectileType<ObeSadeeToothProjectile>(), 20, 0f, Main.myPlayer);
                }
            }
        }

        private void SummonHealerMinion()
        {
            int count = isExpert ? 2 : 1;
            for (int i = 0; i < count; i++)
            {
                int id = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ObeSadeeHealerMinion>());
                if (id < Main.maxNPCs)
                {
                    Main.npc[id].ai[0] = NPC.whoAmI;
                    Main.npc[id].ai[1] = MathHelper.ToRadians(i * (360f / count));
                    if (Main.netMode != NetmodeID.SinglePlayer)
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            int totalFrames = animationType switch { 0 => 5, 1 => 9, 2 => 13, _ => 1 };
            int frameWidth = texture.Width / totalFrames;
            int row = animationType;

            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.X += frameWidth;
                if (NPC.frame.X >= frameWidth * totalFrames)
                    NPC.frame.X = 0;
            }

            NPC.frame.Y = frameHeight * row;
            NPC.frame.Width = frameWidth;
            NPC.frame.Height = frameHeight;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<ObeSadeeBag>()));
            
            // Boss healing potion drop
            npcLoot.Add(ItemDropRule.Common(ItemID.HealingPotion, 1, 5, 15));
        }

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedObeSadee, -1);
        }


    }
}

