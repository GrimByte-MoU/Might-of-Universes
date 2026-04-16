using System.IO;
using Terraria.GameContent.Bestiary;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.NPCs
{
    internal class GummyWormHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<GummyWormBody>();
        public override int TailType => ModContent.NPCType<GummyWormTail>();

        internal static readonly Color[] WormColors = new Color[]
        {
            new Color(255, 80,  80),
            new Color(255, 160, 40),
            new Color(255, 230, 40),
            new Color(80,  200, 80),
            new Color(60,  160, 255),
            new Color(160, 80,  255),
            new Color(255, 100, 200),
        };

        public override void SetStaticDefaults()
        {
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "MightofUniverses/Content/NPCs/Enemies/GummyWorm_Bestiary",
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.aiStyle = -1;
            NPC.damage = 18;
            NPC.defense = 4;
            NPC.lifeMax = 80;
            NPC.value = 60f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = Type;
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.localAI[2] = Main.rand.Next(WormColors.Length);
            NPC.netUpdate = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("A worm made entirely of gummy candy. It wriggles endlessly through the sugary soil.")
            ]);
        }

        public override void Init()
        {
            MinSegmentLength = 6;
            MaxSegmentLength = 10;
            CommonWormInit(this);
        }

        internal static void CommonWormInit(Worm worm)
        {
            worm.MoveSpeed = 5f;
            worm.Acceleration = 0.04f;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return WormColors[(int)NPC.localAI[2] % WormColors.Length];
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[2]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[2] = reader.ReadSingle();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<SugarCrash>(), 120);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(
                ModContent.ItemType<GummyMembrane>(), 1, 2, 4));
        }
    }

    internal class GummyWormBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<GummyWormHead>();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerBody);
            NPC.aiStyle = -1;
            NPC.damage = 12;
            NPC.defense = 6;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = ModContent.NPCType<GummyWormHead>();
        }

        public override void Init()
        {
            GummyWormHead.CommonWormInit(this);
        }

        public override Color? GetAlpha(Color drawColor)
        {
            NPC head = Main.npc[NPC.realLife];
            if (!head.active) return null;
            return GummyWormHead.WormColors[(int)head.localAI[2] % GummyWormHead.WormColors.Length];
        }
    }

    internal class GummyWormTail : WormTail
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<GummyWormHead>();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerTail);
            NPC.aiStyle = -1;
            NPC.damage = 8;
            NPC.defense = 8;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = ModContent.NPCType<GummyWormHead>();
        }

        public override void Init()
        {
            GummyWormHead.CommonWormInit(this);
        }
        
        public override Color? GetAlpha(Color drawColor)
        {
            NPC head = Main.npc[NPC.realLife];
            if (!head.active) return null;
            return GummyWormHead.WormColors[(int)head.localAI[2] % GummyWormHead.WormColors.Length];
        }
    }
}