using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MightofUniverses.Common.Players
{
    public class TatteredVeilPlayer : ModPlayer
    {
        public bool usedTatteredVeil;

        public override void UpdateEquips()
        {
            if (usedTatteredVeil)
            {
                Player.moveSpeed += 0.20f;
                Player.runAcceleration *= 1.5f;
                Player.runSlowdown *= 1.5f;
                Player.maxFallSpeed *= 1.5f;
                Player.wingTimeMax += 60;
                Player.extraAccessorySlots += 1;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["usedTatteredVeil"] = usedTatteredVeil;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("usedTatteredVeil"))
                usedTatteredVeil = tag.GetBool("usedTatteredVeil");
        }
    }
}
