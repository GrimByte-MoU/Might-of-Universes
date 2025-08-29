using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class PrismaticSpherePlayer : ModPlayer
    {
        public bool hasPrismaticSphere;

        public override void ResetEffects()
        {
            hasPrismaticSphere = false;
        }
    }
}