using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using System.Collections.Generic;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class PlanteraBuffsGlobalNPC : GlobalNPC
    {
        private static readonly HashSet<int> PlanteraBuffedNPCs = new()
        {
            NPCID.BloodEelBody,
            NPCID.BloodEelHead,
            NPCID.BloodEelTail,
            NPCID.BloodNautilus,
            NPCID.BloodSquid,
            NPCID.BloodZombie,
            NPCID.Clown,
            NPCID.CorruptBunny,
            NPCID.CorruptGoldfish,
            NPCID.CorruptPenguin,
            NPCID.CrimsonBunny,
            NPCID.CrimsonGoldfish,
            NPCID.CrimsonPenguin,
            NPCID.Drippler,
            NPCID.EyeballFlyingFish,
            NPCID.GoblinArcher,
            NPCID.GoblinPeon,
            NPCID.GoblinShark,
            NPCID.GoblinSummoner,
            NPCID.GoblinThief,
            NPCID.GoblinWarrior,
            NPCID.MisterStabby,
            NPCID.MoonLordCore,
            NPCID.Parrot,
            NPCID.PirateCaptain,
            NPCID.PirateCorsair,
            NPCID.PirateCrossbower,
            NPCID.PirateDeadeye,
            NPCID.PirateDeckhand,
            NPCID.PirateGhost,
            NPCID.PirateShip,
            NPCID.PirateShipCannon,
            NPCID.Plantera,
            NPCID.ShadowFlameApparition,
            NPCID.SnowBalla,
            NPCID.SnowmanGangsta,
            NPCID.TheBride,
            NPCID.TheGroom,
            NPCID.ZombieMerman
        };

        public override void AI(NPC npc)
        {
            if (NPC.downedPlantBoss && PlanteraBuffedNPCs.Contains(npc.type) && npc.localAI[1] == 0f)
            {
                
                npc.velocity *= 1.5f;
                npc.localAI[1] = 1f;
            }
        }
    }
}
