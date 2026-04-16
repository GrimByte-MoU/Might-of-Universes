using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses.Content.NPCs
{
    public class Lollicopter : ModNPC
    {


        private const int AttackCooldown = 180;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 15;
            NPC.defense = 4;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 50f;
            NPC.knockBackResist = 0.3f;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.aiStyle = -1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A lollipop spinning so fast it achieved flight. It pelts enemies with candy.")
            });
        }

        public override void AI()
        {
            Player target = Main.player[NPC.target];

            if (NPC.target < 0 || NPC.target == 255 || target.dead || !target.active)
            {
                NPC.TargetClosest(true);
                target = Main.player[NPC.target];
            }

            float hoverY = target.Center.Y - 180f;
            float hoverX = target.Center.X + (NPC.Center.X < target.Center.X ? -200f : 200f);

            Vector2 hoverPos = new Vector2(hoverX, hoverY);
            Vector2 toHover = hoverPos - NPC.Center;

            float hoverSpeed = 4f;
            if (toHover.Length() > hoverSpeed)
                toHover.Normalize();

            NPC.velocity = Vector2.Lerp(NPC.velocity, toHover * hoverSpeed, 0.05f);

            NPC.ai[2]++;
            NPC.velocity.Y += (float)System.Math.Sin(NPC.ai[2] * 0.05f) * 0.05f;

            NPC.spriteDirection = NPC.Center.X < target.Center.X ? 1 : -1;

            NPC.ai[0]++;
            if (NPC.ai[0] >= AttackCooldown)
            {
                NPC.ai[0] = 0;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float direction = target.Center.X > NPC.Center.X ? 1f : -1f;
                    Vector2 spawnPos = new Vector2(NPC.Center.X, NPC.Center.Y + 20f);
                    Vector2 velocity = new Vector2(direction * 5f, 2f);
                    Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<LollicopterPop>(),
                        NPC.damage / 2,
                        2f
                    );
                }

                NPC.netUpdate = true;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(
                ModContent.ItemType<GummyMembrane>(), 1, 1, 3));

            npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(
                ModContent.ItemType<SweetTooth>(), 1, 1, 3));
        }
    }
}