using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class CyberOneWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(900, 12f, 5f, true, 15f, 7f);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 0;
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
            Item.scale = 2f;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.95f;
            ascentWhenRising = 0.25f;
            maxCanAscendMultiplier = 1.5f;
            maxAscentMultiplier = 4f;
            constantAscend = 0.15f;
        }
    }
}