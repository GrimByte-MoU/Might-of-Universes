using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class CavityNecklace : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 5;
            
            if (player.HasBuff(ModContent.BuffType<Hyper>()))
            {
                player.GetArmorPenetration(DamageClass.Generic) += 5;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SharkToothNecklace)
                .AddIngredient(ModContent.ItemType<SweetTooth>(), 5)
                .AddIngredient(ModContent.ItemType<GummyMembrane>(), 10)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
