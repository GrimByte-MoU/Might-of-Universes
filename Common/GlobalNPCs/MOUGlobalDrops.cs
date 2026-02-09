using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using MightofUniverses.Content.Items.Accessories;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class MOUGlobalDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Antlion || npc.type == NPCID.WalkingAntlion || npc.type == NPCID.FlyingAntlion)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentoftheDesert>(), 11));
            }

            if (npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<UnderworldCoin>(), 16));
            }

            if (npc.type == NPCID.Harpy)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SkyRibbon>(), 10));
            }

            if (npc.type == NPCID.BoneSerpentHead)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FireheartShard>(), 10));
            }
        }
        public override void OnKill(NPC npc)
        {
                        if (Main.hardMode && !npc.SpawnedFromStatue && (npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon || npc.type == NPCID.BoneSerpentHead))
                        {
                            if (Main.rand.Next(10) < 3)
                {
                    int stack = Main.rand.Next(1, 4);
                    Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ModContent.ItemType<DemonicEssence>(), stack);
                }
            }
        }
    }
}