using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Dusts
{
    public class MintDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale = 1.5f;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.15f;
            dust.scale *= 0.98f;

            // Mint green color light
            Lighting.AddLight(dust.position, 0.1f, 0.8f, 0.4f);

            if (dust.scale < 0.5f)
            {
                dust.active = false;
            }

            return false;
        }
    }
}
