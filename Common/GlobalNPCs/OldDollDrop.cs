using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class OldDollDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Zombie ||
                npc.type == NPCID.BaldZombie ||
                npc.type == NPCID.PincushionZombie ||
                npc.type == NPCID.SlimedZombie ||
                npc.type == NPCID.SwampZombie ||
                npc.type == NPCID.TwiggyZombie ||
                npc.type == NPCID.FemaleZombie ||
                npc.type == NPCID.ZombieEskimo ||
                npc.type == NPCID.ArmedZombie ||
                npc.type == NPCID.ArmedZombieCenx ||
                npc.type == NPCID.ArmedZombieSlimed ||
                npc.type == NPCID.ArmedZombieSwamp ||
                npc.type == NPCID.ArmedZombieTwiggy)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldDoll>(), 20));
            }
        }
    }
}