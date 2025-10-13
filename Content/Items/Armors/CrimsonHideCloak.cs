using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class CrimsonHideCloak : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) *= 0.80f; // -20% all damage
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.30f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 17)
                .AddIngredient(ItemID.TissueSample, 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}