using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.NPCs
{
    public class CavityBat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 28;
            NPC.height = 18;
            NPC.damage = 5;
            NPC.defense = 1;
            NPC.lifeMax = 25;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 20f;
            NPC.knockBackResist = 0.75f;
            NPC.noGravity = true;
            NPC.aiStyle = NPCAIStyleID.Bat;
            AIType = NPCID.CaveBat;
            AnimationType = NPCID.CaveBat;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("A bat with a mouthful of cavities. Its bite is worse than its bark.")
            });
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<SugarCrash>(), 60);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(
                ModContent.ItemType<GummyMembrane>(), 3, 1, 2));

            npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(
                ModContent.ItemType<SweetTooth>(), 3, 1, 2));
        }
    }
}