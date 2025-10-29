using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Buffs
{
    // While active, handled in the scythe's ModifyHitNPC:
    // - +50 armor penetration
    // - +30% damage (source multiplier)
    public class PrimalSavageryBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            Lighting.AddLight(player.Center, new Vector3(0.15f, 0.12f, 0.08f));
            player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) += 0.30f;
            player.GetArmorPenetration(ModContent.GetInstance<ReaperDamageClass>()) += 50;
        }
    }
}