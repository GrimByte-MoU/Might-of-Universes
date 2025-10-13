using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class CorruptScaleBoots : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 40);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) *= 0.80f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.25f;
            player.moveSpeed += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 13)
                .AddIngredient(ItemID.ShadowScale, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}