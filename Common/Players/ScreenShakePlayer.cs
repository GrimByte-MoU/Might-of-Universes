using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class ScreenShakePlayer : ModPlayer
    {
        private int shakeTimer = 0;
        private float shakeIntensity = 0f;

        public void ScreenShake(float intensity, int duration)
        {
            shakeIntensity = intensity;
            shakeTimer = duration;
        }

        public override void ModifyScreenPosition()
        {
            if (shakeTimer > 0)
            {
                Main.screenPosition += Main.rand.NextVector2Circular(shakeIntensity, shakeIntensity);
                shakeTimer--;
                shakeIntensity *= 0.95f;
            }
        }
    }
}