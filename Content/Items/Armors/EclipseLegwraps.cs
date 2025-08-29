using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
	[AutoloadEquip(EquipType.Legs)]
	public class EclipseLegwraps : ModItem
	{

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 5); // How many coins the item is worth
			Item.rare = ItemRarityID.Lime; // The rarity of the item
			Item.defense = 5; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
            ReaperPlayer reaperPlayer = player.GetModPlayer<ReaperPlayer>();
			player.moveSpeed *= 1.2f; 
            reaperPlayer.reaperDamageMultiplier += 0.10f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SolunarBreastplate>())
                .AddIngredient(ModContent.ItemType<EclipseLight>(), 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}