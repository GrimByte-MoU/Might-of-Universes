using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Buffs
{
    public class FaesTrade : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false; // It's a buff, not a debuff
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Increase damage by 50%
            player.GetDamage(DamageClass.Generic) += 0.5f;
            
            // Increase damage taken by 100%
            player.endurance -= 0.5f; // Reduces damage reduction by 50%, effectively increasing damage taken by 100%
            
            // Visual effect - fairy-like dust
            if (Main.rand.NextBool(3))
            {
                Color dustColor = new Color(
                    Main.rand.Next(150, 255),
                    Main.rand.Next(150, 255),
                    Main.rand.Next(150, 255)
                );
                
                Dust dust = Dust.NewDustDirect(
                    player.position,
                    player.width,
                    player.height,
                    DustID.RainbowTorch,
                    0f,
                    0f,
                    100,
                    dustColor,
                    0.8f
                );
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
                dust.velocity *= 0.3f;
            }
        }
    }
}
