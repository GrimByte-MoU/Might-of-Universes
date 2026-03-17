using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class CoreSpherePlayer : ModPlayer
    {
        public bool hasCoreSphere = false;

        public override void ResetEffects()
        {
            hasCoreSphere = false;
        }
    }
}