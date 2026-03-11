using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Common.Players
{
    public class ScreenShakePlayer : ModPlayer
    {
        public float shakeIntensity = 0f;

        public void AddShake(float intensity)
        {
            shakeIntensity += intensity;
        }

        public override void ModifyScreenPosition()
        {
            if (shakeIntensity > 0f)
            {
                Main.screenPosition += Main.rand.NextVector2Circular(shakeIntensity, shakeIntensity);
                shakeIntensity *= 0.9f;

                if (shakeIntensity < 0.1f)
                    shakeIntensity = 0f;
            }
        }
    }
}