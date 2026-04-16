using Terraria.GameContent.Bestiary;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.NPCs
{
    public class Jawbreaker : ModNPC
    {
        private static readonly Color[] CandyColors = new Color[]
        {
            new Color(255, 80,  80),
            new Color(255, 160, 40),
            new Color(255, 230, 40),
            new Color(80,  200, 80),
            new Color(60,  160, 255),
            new Color(160, 80,  255),
            new Color(255, 100, 200),
        };

        private Color MyColor => CandyColors[(int)NPC.ai[3] % CandyColors.Length];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 25;
            NPC.defense = 14;
            NPC.lifeMax = 120;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 55f;
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = NPCAIStyleID.Unicorn;
            AIType = NPCID.Unicorn;
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.ai[3] = Main.rand.Next(CandyColors.Length);
            NPC.netUpdate = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A dense sphere of hardened candy. It has no eyes, no mouth, and absolutely no intention of stopping.")
            });
        }

        public override void AI()
        {
            NPC.rotation += NPC.velocity.X * 0.05f;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return MyColor;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<SugarCrash>(), 120);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(
                ModContent.ItemType<GummyMembrane>(), 2, 1, 2));

            npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(
                ModContent.ItemType<SweetTooth>(), 2, 1, 2));
        }
    }
}