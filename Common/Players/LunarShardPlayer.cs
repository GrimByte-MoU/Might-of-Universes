using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace MightofUniverses.Common.Players
{
    public class LunarShardPlayer : ModPlayer
    {
        public bool consumedLunarCoreShard;

        public override void UpdateEquips()
        {
            if (consumedLunarCoreShard)
            {
                Player.extraAccessorySlots += 1;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["consumedLunarCoreShard"] = consumedLunarCoreShard;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("consumedLunarCoreShard"))
                consumedLunarCoreShard = tag.GetBool("consumedLunarCoreShard");
        }
    }
}
