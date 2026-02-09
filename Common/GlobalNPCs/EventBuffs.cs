using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using System.Collections.Generic;

namespace MightofUniverses.Content.NPCs
{
    public class EventBuffs : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            {
                if (npc.type == NPCID.Plantera)
                {
                Main.NewText("The jungle's curse is broken!", 0, 175, 0);
                Main.NewText("The pirates no longer fear the jungle", 255, 215, 0);
                Main.NewText("The bloody moon awaits a slaughter", 255, 0, 0);
                Main.NewText("The frost legion grows ever colder", 0, 191, 255);
                }
                
                if (npc.type == NPCID.MoonLordCore)
                {
            Main.NewText("The moon tyrant's essence flows across the land!", 255, 191, 255);
                Main.NewText("Beings of ancient stir and awaken, free from their shackles", 255, 0, 0);
                Main.NewText("Elemental phenomena begin to occur", 128, 128, 100);
                Main.NewText("The gaelic invaders sense your might", 188, 139, 194);
                Main.NewText("The pirates have successfully awakened their god", 176, 156, 39);
                Main.NewText("Something stirs deep within the ocean", 4, 209, 249);
                
                }
            }
        }

        public override void SetDefaults(NPC npc)
        {
            if (NPC.downedPlantBoss)
            {
                if (npc.type == NPCID.BloodZombie)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 3000;
                        npc.damage = 125;
                        npc.defense = 50;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 2000;
                        npc.damage = 100;
                        npc.defense = 40;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1000;
                        npc.damage = 80;
                        npc.defense = 30;
                    }
                }
                else if (npc.type == NPCID.Drippler)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 2000;
                        npc.damage = 135;
                        npc.defense = 55;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 1800;
                        npc.damage = 110;
                        npc.defense = 45;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 900;
                        npc.damage = 85;
                        npc.defense = 35;
                    }
                }
                else if (npc.type == NPCID.TheGroom)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 3500;
                        npc.damage = 175;
                        npc.defense = 50;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 2500;
                        npc.damage = 150;
                        npc.defense = 40;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1500;
                        npc.damage = 120;
                        npc.defense = 30;
                    }
                }
                else if (npc.type == NPCID.TheBride)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 3500;
                        npc.damage = 175;
                        npc.defense = 50;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 2500;
                        npc.damage = 150;
                        npc.defense = 40;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1500;
                        npc.damage = 120;
                        npc.defense = 30;
                    }
                }
                else if (npc.type == NPCID.CorruptBunny || npc.type == NPCID.CrimsonBunny)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 1750;
                        npc.damage = 120;
                        npc.defense = 40;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 1000;
                        npc.damage = 100;
                        npc.defense = 30;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 750;
                        npc.damage = 80;
                        npc.defense = 20;
                    }
                }
                else if (npc.type == NPCID.CorruptGoldfish || npc.type == NPCID.CrimsonGoldfish)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 1500;
                        npc.damage = 140;
                        npc.defense = 50;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 1150;
                        npc.damage = 110;
                        npc.defense = 35;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 800;
                        npc.damage = 85;
                        npc.defense = 25;
                    }
                }
                else if (npc.type == NPCID.CorruptPenguin || npc.type == NPCID.CrimsonPenguin)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 1750;
                        npc.damage = 120;
                        npc.defense = 40;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 1000;
                        npc.damage = 100;
                        npc.defense = 30;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 750;
                        npc.damage = 80;
                        npc.defense = 20;
                    }
                }
                else if (npc.type == NPCID.EyeballFlyingFish)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 10000;
                        npc.damage = 175;
                        npc.defense = 50;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 7500;
                        npc.damage = 150;
                        npc.defense = 45;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 5000;
                        npc.damage = 125;
                        npc.defense = 40;
                    }
                }
                else if (npc.type == NPCID.ZombieMerman)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 15000;
                        npc.damage = 230;
                        npc.defense = 70;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 10000;
                        npc.damage = 180;
                        npc.defense = 60;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 5000;
                        npc.damage = 130;
                        npc.defense = 50;
                    }
                }
                else if (npc.type == NPCID.Clown)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 5000;
                        npc.damage = 200;
                        npc.defense = 100;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 4000;
                        npc.damage = 165;
                        npc.defense = 60;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 3000;
                        npc.damage = 140;
                        npc.defense = 50;
                    }
                }
                else if (npc.type == NPCID.GoblinShark)
                {
                    if (Main.masterMode)
                    {
                        npc.damage = 220;
                        npc.defense = 70;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.damage = 170;
                        npc.defense = 60;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.damage = 120;
                        npc.defense = 50;
                    }
                }
                else if (npc.type == NPCID.BloodEelHead)
                {
                    if (Main.masterMode)
                    {
                        npc.damage = 220;
                        npc.defense = 60;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.damage = 170;
                        npc.defense = 50;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.damage = 120;
                        npc.defense = 40;
                    }
                }
                else if (npc.type == NPCID.BloodEelBody)
                {
                    if (Main.masterMode)
                    {
                        npc.damage = 190;
                        npc.defense = 70;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.damage = 145;
                        npc.defense = 60;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.damage = 100;
                        npc.defense = 50;
                    }
                }
                else if (npc.type == NPCID.BloodEelTail)
                {
                    if (Main.masterMode)
                    {
                        npc.damage = 140;
                        npc.defense = 50;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.damage = 115;
                        npc.defense = 35;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.damage = 80;
                        npc.defense = 20;
                    }
                }
                else if (npc.type == NPCID.BloodSquid)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 3250;
                        npc.damage = 220;
                        npc.defense = 65;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 2250;
                        npc.damage = 160;
                        npc.defense = 55;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1250;
                        npc.damage = 120;
                    }
                }
                // Frost Legion Enemies
                else if (npc.type == NPCID.SnowmanGangsta)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 2000;
                        npc.damage = 180;
                        npc.defense = 65;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 1500;
                        npc.damage = 130;
                        npc.defense = 55;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1000;
                        npc.damage = 110;
                        npc.defense = 45;
                    }
                }
                else if (npc.type == NPCID.MisterStabby)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 2300;
                        npc.damage = 220;
                        npc.defense = 65;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 1800;
                        npc.damage = 170;
                        npc.defense = 55;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1300;
                        npc.damage = 140;
                        npc.defense = 45;
                    }
                }
                else if (npc.type == NPCID.SnowBalla)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 2100;
                        npc.damage = 175;
                        npc.defense = 65;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 1650;
                        npc.damage = 130;
                        npc.defense = 55;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1200;
                        npc.damage = 100;
                        npc.defense = 45;
                    }
                }

                // Pirate Invasion Enemies
                else if (npc.type == NPCID.PirateDeckhand)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 2400;
                        npc.damage = 185;
                        npc.defense = 65;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 1800;
                        npc.damage = 150;
                        npc.defense = 55;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1200;
                        npc.damage = 130;
                        npc.defense = 45;
                    }
                }

                else if (npc.type == NPCID.PirateCorsair)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 2900;
                        npc.damage = 210;
                        npc.defense = 70;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 2200;
                        npc.damage = 180;
                        npc.defense = 60;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1500;
                        npc.damage = 150;
                        npc.defense = 50;
                    }
                }
                else if (npc.type == NPCID.PirateDeadeye)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 2500;
                        npc.damage = 200;
                        npc.defense = 60;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 1900;
                        npc.damage = 135;
                        npc.defense = 50;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1300;
                        npc.damage = 110;
                        npc.defense = 40;
                    }
                }
                else if (npc.type == NPCID.PirateCrossbower)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 2500;
                        npc.damage = 205;
                        npc.defense = 75;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 1750;
                        npc.damage = 160;
                        npc.defense = 60;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 1200;
                        npc.damage = 125;
                        npc.defense = 45;
                    }
                }
                else if (npc.type == NPCID.PirateCaptain)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 14000;
                        npc.damage = 230;
                        npc.defense = 75;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 9000;
                        npc.damage = 175;
                        npc.defense = 65;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 5000;
                        npc.damage = 100;
                        npc.defense = 55;
                    }
                }
                else if (npc.type == NPCID.Parrot)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 500;
                        npc.damage = 235;
                        npc.defense = 30;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 400;
                        npc.damage = 150;
                        npc.defense = 25;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 300;
                        npc.damage = 100;
                        npc.defense = 20;
                    }
                }
                else if (npc.type == NPCID.PirateGhost)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 2250;
                        npc.damage = 275;
                        npc.defense = 70;
                       
                    }
                    else if (Main.expertMode)
                    {
                    npc.lifeMax = 1500;
                    npc.damage = 155;
                    npc.defense = 60;
                    npc.velocity.X = 1.25f; 
                    npc.velocity.Y = 1.25f;
                    }
                    else  
                    {
                    npc.lifeMax = 750;
                    npc.damage = 110;
                    npc.defense = 50;
                    }
                }

                // Event Bosses
                else if (npc.type == NPCID.BloodNautilus)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 30000;
                        npc.damage = 200;
                        npc.defense = 70;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 22500;
                        npc.damage = 160;
                        npc.defense = 60;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 15000;
                        npc.damage = 120;
                        npc.defense = 50;
                    }
                }
                else if (npc.type == NPCID.PirateShip)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 4000;
                        npc.defense = 70;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 3000;
                        npc.defense = 60;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 2000;
                        npc.defense = 55;
                    }
                }
                else if (npc.type == NPCID.PirateShipCannon)
                {
                    if (Main.masterMode)
                    {
                        npc.lifeMax = 9000;
                        npc.damage = 200;
                        npc.defense = 70;
                       
                    }
                    else if (Main.expertMode)
                    {
                        npc.lifeMax = 6000;
                        npc.damage = 150;
                        npc.defense = 60;
                        npc.velocity.X *= 1.25f;
                        npc.velocity.Y *= 1.25f;
                    }
                    else
                    {
                        npc.lifeMax = 3000;
                        npc.damage = 100;
                        npc.defense = 50;
                    }
                }
            }
                
                if (NPC.downedMoonlord)
                {
                    // Enhanced Goblin Army
                    if (npc.type == NPCID.GoblinPeon)
                    {
                    if (Main.masterMode)
                        {
                            npc.lifeMax = 6000;
                            npc.damage = 300;
                            npc.defense = 110;
                            npc.velocity.X *= 1.5f;
                            npc.velocity.Y *= 1.5f;
                        }
                        else if (Main.expertMode)
                        {
                            npc.lifeMax = 4500;
                            npc.damage = 190;
                            npc.defense = 95;
                            npc.velocity.X *= 1.25f;
                            npc.velocity.Y *= 1.25f;
                        }
                        else
                        {
                            npc.lifeMax = 3000;
                            npc.damage = 160;
                            npc.defense = 80;
                        }
                    }
                    else if (npc.type == NPCID.GoblinThief)
                    {
                        if (Main.masterMode)
                        {
                            npc.lifeMax = 5050;
                            npc.damage = 310;
                            npc.defense = 100;
                            npc.velocity.X *= 1.5f;
                            npc.velocity.Y *= 1.5f;
                        }
                        else if (Main.expertMode)
                        {
                            npc.lifeMax = 3900;
                            npc.damage = 205;
                            npc.defense = 80;
                            npc.velocity.X *= 1.25f;
                            npc.velocity.Y *= 1.25f;
                        }
                        else
                        {
                            npc.lifeMax = 2750;
                            npc.damage = 160;
                            npc.defense = 60;
                        }
                    }
                    else if (npc.type == NPCID.GoblinArcher)
                    {
                        if (Main.masterMode)
                        {
                            npc.lifeMax = 4700;
                            npc.damage = 305;
                            npc.defense = 75;
                            npc.velocity.X *= 1.5f;
                            npc.velocity.Y *= 1.5f;
                        }
                        else if (Main.expertMode)
                        {
                            npc.lifeMax = 3600;
                            npc.damage = 225;
                            npc.defense = 65;
                            npc.velocity.X *= 1.25f;
                            npc.velocity.Y *= 1.25f;
                        }
                        else
                        {
                            npc.lifeMax = 2500;
                            npc.damage = 175;
                            npc.defense = 55;
                        }
                    }
                    else if (npc.type == NPCID.GoblinWarrior)
                    {
                        if (Main.masterMode)
                        {
                            npc.lifeMax = 7500;
                            npc.damage = 240;
                            npc.defense = 135;
                            npc.velocity.X *= 1.5f;
                            npc.velocity.Y *= 1.5f;
                        }
                        else if (Main.expertMode)
                        {
                            npc.lifeMax = 5000;
                            npc.damage = 170;
                            npc.defense = 110;
                            npc.velocity.X *= 1.25f;
                            npc.velocity.Y *= 1.25f;
                        }
                        else
                        {
                            npc.lifeMax = 4000;
                            npc.damage = 120;
                            npc.defense = 85;
                        }
                    }
                    else if (npc.type == NPCID.GoblinSummoner)
                    {
                        if (Main.masterMode)
                        {
                            npc.lifeMax = 27500;
                            npc.damage = 360;
                            npc.defense = 130;
                            npc.velocity.X *= 1.5f;
                            npc.velocity.Y *= 1.5f;
                        }
                        else if (Main.expertMode)
                        {
                            npc.lifeMax = 18000;
                            npc.damage = 230;
                            npc.defense = 105;
                            npc.velocity.X *= 1.25f;
                            npc.velocity.Y *= 1.25f;
                        }
                        else
                        {
                            npc.lifeMax = 10000;
                            npc.damage = 185;
                            npc.defense = 80;
                        }
                    }
                    else if (npc.type == NPCID.ShadowFlameApparition)
                    {
                        if (Main.masterMode)
                        {
                            npc.lifeMax = 2000;
                            npc.damage = 300;
                            npc.defense = 60;
                            npc.velocity.X *= 1.5f;
                            npc.velocity.Y *= 1.5f;
                        }
                        else if (Main.expertMode)
                        {
                            npc.lifeMax = 1500;
                            npc.damage = 210;
                            npc.defense = 50;
                            npc.velocity.X *= 1.25f;
                            npc.velocity.Y *= 1.25f;
                        }
                        else
                        {
                            npc.lifeMax = 1000;
                            npc.damage = 170;
                            npc.defense = 40;
                        }
                    }
                    // Enhanced Pirate Invasion
                    if (npc.type == NPCID.PirateDeckhand)
                    {
                        if (Main.masterMode)
                        {
                            npc.lifeMax = 6000;
                            npc.damage = 300;
                            npc.defense = 110;
                            npc.velocity.X *= 1.5f;
                            npc.velocity.Y *= 1.5f;
                        }
                        else if (Main.expertMode)
                        {
                            npc.lifeMax = 4500;
                            npc.damage = 220;
                            npc.defense = 95;
                            npc.velocity.X *= 1.25f;
                            npc.velocity.Y *= 1.25f;
                        }
                        else
                        {
                            npc.lifeMax = 3000;
                            npc.damage = 185;
                            npc.defense = 80;
                        }
                    }
                    else if (npc.type == NPCID.PirateCorsair)
                    {
                        if (Main.masterMode)
                        {
                            npc.lifeMax = 6250;
                            npc.damage = 375;
                            npc.defense = 110;
                            npc.velocity.X *= 1.5f;
                            npc.velocity.Y *= 1.5f;
                        }
                        else if (Main.expertMode)
                        {
                            npc.lifeMax = 5000;
                            npc.damage = 235;
                            npc.defense = 100;
                            npc.velocity.X *= 1.25f;
                            npc.velocity.Y *= 1.25f;
                        }
                        else
                        {
                            npc.lifeMax = 3750;
                            npc.damage = 190;
                            npc.defense = 90;
                        }
                    }
                    else if (npc.type == NPCID.PirateDeadeye)
                    {
                        if (Main.masterMode)
                        {
                            npc.lifeMax = 4750;
                            npc.damage = 360;
                            npc.defense = 105;
                            npc.velocity.X *= 1.5f;
                            npc.velocity.Y *= 1.5f;
                        }
                        else if (Main.expertMode)
                        {
                            npc.lifeMax = 4000;
                            npc.damage = 215;
                            npc.defense = 90;
                            npc.velocity.X *= 1.25f;
                            npc.velocity.Y *= 1.25f;
                        }
                        else
                        {
                            npc.lifeMax = 3250;
                            npc.damage = 175;
                            npc.defense = 75;
                        }
                    }
                    else if (npc.type == NPCID.PirateCrossbower)
                    {
                        if (Main.masterMode)
                        {
                            npc.lifeMax = 4500;
                            npc.damage = 375;
                            npc.defense = 90;
                            npc.velocity.X *= 1.5f;
                            npc.velocity.Y *= 1.5f;
                        }
                        else if (Main.expertMode)
                        {
                            npc.lifeMax = 3900;
                            npc.damage = 240;
                            npc.defense = 80;
                            npc.velocity.X *= 1.25f;
                            npc.velocity.Y *= 1.25f;
                        }
                        else
                        {
                            npc.lifeMax = 3300;
                            npc.damage = 190;
                            npc.defense = 70;
                        }
                    }
                else if (npc.type == NPCID.PirateCaptain)
                {
                     if (Main.masterMode)
                    {
                    npc.lifeMax = 25000;
                    npc.damage = 400;
                    npc.defense = 125;
                    npc.velocity.X = 1.5f;
                    npc.velocity.Y = 1.5f;
                    }
                    else if (Main.expertMode)
                    {
                    npc.lifeMax = 17500;
                    npc.damage = 290;
                    npc.defense = 100;
                    npc.velocity.X = 1.25f;
                    npc.velocity.Y = 1.25f;
                    }
                    else  
                    {
                    npc.lifeMax = 10000;
                    npc.damage = 215;
                    npc.defense = 75;
                    }
                }
                else if (npc.type == NPCID.Parrot)
                {
                     if (Main.masterMode)
                    {
                    npc.lifeMax = 2000;
                    npc.damage = 380;
                    npc.defense = 30;
                    npc.velocity.X = 1.5f;
                    npc.velocity.Y = 1.5f;
                    }
                    else if (Main.expertMode)
                    {
                    npc.lifeMax = 1500;
                    npc.damage = 280;
                    npc.defense = 25;
                    npc.velocity.X = 1.25f;
                    npc.velocity.Y = 1.25f;
                    }
                    else  
                    {
                    npc.lifeMax = 1000;
                    npc.damage = 175;
                    npc.defense = 20;
                    }
                }
                else if (npc.type == NPCID.PirateGhost)
                {
                     if (Main.masterMode)
                    {
                    npc.lifeMax = 4000;
                    npc.damage = 380;
                    npc.defense = 70;
                    npc.velocity.X = 1.5f;
                    npc.velocity.Y = 1.5f;
                    }
                    else if (Main.expertMode)
                    {
                    npc.lifeMax = 3000;
                    npc.damage = 280;
                    npc.defense = 60;
                    npc.velocity.X = 1.25f;
                    npc.velocity.Y = 1.25f;
                    }
                    else  
                    {
                    npc.lifeMax = 2000;
                    npc.damage = 175;
                    npc.defense = 50;
                    }
                }
                else if (npc.type == NPCID.PirateShip)
                {
                     if (Main.masterMode)
                    {
                    npc.lifeMax = 7500;
                    npc.defense = 70;
                    npc.velocity.X = 1.5f;
                    npc.velocity.Y = 1.5f;
                    }
                    else if (Main.expertMode)
                    {
                    npc.lifeMax = 6250;
                    npc.defense = 60;
                    npc.velocity.X = 1.25f;
                    npc.velocity.Y = 1.25f;
                    }
                    else  
                    {
                    npc.lifeMax = 5000;
                    npc.defense = 55;
                    }
                }
                else if (npc.type == NPCID.PirateShipCannon)
                {
                     if (Main.masterMode)
                    {
                    npc.lifeMax = 20000;
                    npc.damage = 390;
                    npc.defense = 125;
                    npc.velocity.X = 1.5f;
                    npc.velocity.Y = 1.5f;
                    }
                    else if (Main.expertMode)
                    {
                    npc.lifeMax = 15000;
                    npc.damage = 275;
                    npc.defense = 110;
                    npc.velocity.X = 1.25f;
                    npc.velocity.Y = 1.25f;
                    }
                    else  
                    {
                    npc.lifeMax = 10000;
                    npc.damage = 200;
                    npc.defense = 100;
                    }
                }
            }
        }

       public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
{
    if (NPC.downedPlantBoss)
    {
        if (npc.type == NPCID.PirateCaptain)
        {
            if (projectile.type == ProjectileID.CannonballFriendly)
            {
                modifiers.SourceDamage *= 2f;
            }
            if (projectile.type == ProjectileID.BulletDeadeye)
            {
                modifiers.SourceDamage *= 0.5f;
            }
        }
        
        if (npc.type == NPCID.BloodNautilus)
        {
            if (projectile.type == ProjectileID.BloodNautilusShot)
            {
                modifiers.SourceDamage *= 1.5f;
            }
        }
    }
    else if (NPC.downedMoonlord)
    {
        if (npc.type == NPCID.PirateCaptain)
        {
            if (projectile.type == ProjectileID.CannonballFriendly)
            {
                modifiers.SourceDamage *= 2f;
            }
            if (projectile.type == ProjectileID.BulletDeadeye)
            {
                modifiers.SourceDamage *= 0.5f;
            }
        }
    }
}

    }
}

