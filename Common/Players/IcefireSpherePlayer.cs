using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class IcefireSpherePlayer : ModPlayer
    {
        public bool hasIcefireSphere;

        public override void ResetEffects()
        {
            hasIcefireSphere = false;
        }
    }
}