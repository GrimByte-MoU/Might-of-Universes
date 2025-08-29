using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MightofUniverses.Common.Players
{
    public class CoreOfBlindIdiocyPlayer : ModPlayer
    {
        public bool usedCoreOfBlindIdiocy;

        public override void UpdateEquips()
        {
            if (usedCoreOfBlindIdiocy)
            {
                Player.statLifeMax2 += 275;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["usedCoreOfBlindIdiocy"] = usedCoreOfBlindIdiocy;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("usedCoreOfBlindIdiocy"))
                usedCoreOfBlindIdiocy = tag.GetBool("usedCoreOfBlindIdiocy");
        }
    }
}
