using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System.Collections.Generic;

namespace MightofUniverses.Common.Players
{
    public class PearlOfTheAbyssPlayer : ModPlayer
    {
        public bool usedPearlOfTheAbyss;

        public override void PostUpdate()
        {
            if (usedPearlOfTheAbyss)
            {
                // Normalize gravity in low-gravity environments (like outer space)
                if (Player.ZoneSkyHeight)
                {
                    Player.gravity = 0.4f; // Approximate default gravity
                    Player.noFallDmg = true;
                }

                // Bonus oceanic adaptation perks
                Player.gills = true;
                Player.ignoreWater = true;
                Player.accFlipper = true;
                Player.statLifeMax2 += 25;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["usedPearlOfTheAbyss"] = usedPearlOfTheAbyss;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("usedPearlOfTheAbyss"))
                usedPearlOfTheAbyss = tag.GetBool("usedPearlOfTheAbyss");
        }
    }
}

