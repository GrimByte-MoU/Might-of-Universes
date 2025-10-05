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
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			ReaperPlayer reaperPlayer = player.GetModPlayer<ReaperPlayer>();
			player.moveSpeed += 0.2f;
			reaperPlayer.reaperDamageMultiplier += 0.08f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.08f;
			player.GetCritChance(ModContent.GetInstance<ReaperDamageClass>()) += 6f;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SolunarBreastplate>())
				.AddIngredient(ModContent.ItemType<EclipseLight>(), 10)
				.AddIngredient(ItemID.Ectoplasm, 15)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}