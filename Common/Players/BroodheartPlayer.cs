using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MightofUniverses.Common.Players
{
    public class BroodheartPlayer : ModPlayer
    {
        public bool usedBroodheart;

        public override void ResetEffects()
        {
            if (usedBroodheart)
            {
                Player.statLifeMax2 += 25;
                Player.statManaMax2 += 200;
                Player.GetDamage(DamageClass.Generic) += 0.15f;
                Player.maxMinions += 2;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["usedBroodheart"] = usedBroodheart;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("usedBroodheart"))
                usedBroodheart = tag.GetBool("usedBroodheart");
        }
    }
}
