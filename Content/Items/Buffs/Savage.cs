using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common; // ReaperDamageClass

namespace MightofUniverses.Content.Items.Buffs
{
    public class Savage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}