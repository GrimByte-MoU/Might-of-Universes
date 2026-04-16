using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class QuestionableInkFishing : ModPlayer
    {
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            bool inCorruptionOrCrimson = Player.ZoneCorrupt || Player.ZoneCrimson;

            if (!inCorruptionOrCrimson)
                return;

            int power = attempt.fishingLevel;
            float dropChance = 0f;

            if (power >= 100)
                dropChance = 0.20f;
            else if (power >= 50)
                dropChance = 0.10f;
            else if (power >= 25)
                dropChance = 0.05f;

            if (dropChance > 0f && Main.rand.NextFloat() < dropChance)
            {
                itemDrop = ModContent.ItemType<QuestionableInk>();
            }
        }
    }
}