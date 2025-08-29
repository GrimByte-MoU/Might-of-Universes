using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace MightofUniverses.Content.Items.Armors
{
    public class ClockworkPirateWings : ModItem
    {
        public override string Texture => "Terraria/Images/Item_1";
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 0;
        }

        public override void SetDefaults()
        {
            Item.width = 1;
            Item.height = 1;
            Item.accessory = true;
            Item.value = 0;
            Item.rare = -12;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTime = 240;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded) => false;
    }
}

