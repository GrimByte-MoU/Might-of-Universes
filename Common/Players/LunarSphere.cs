using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class LunarSpherePlayer : ModPlayer
    {
        public bool hasLunarSphere;

        public override void ResetEffects()
        {
            hasLunarSphere = false;
        }
    }
}