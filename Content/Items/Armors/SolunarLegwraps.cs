using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
	[AutoloadEquip(EquipType.Legs)]
	public class SolunarLegwraps : ModItem
	{

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player) {
			player.moveSpeed *= 1.05f;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SolunarToken>(), 10)
				.AddIngredient(ModContent.ItemType<DesertWrappings>(), 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}