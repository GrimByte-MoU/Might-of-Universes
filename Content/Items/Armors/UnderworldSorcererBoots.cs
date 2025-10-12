using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class UnderworldSorcererBoots : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1, silver: 75);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.GetCritChance(DamageClass.Magic) += 5f;
            player.moveSpeed += 0.12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 10)
                .AddIngredient(ItemID.SoulofNight, 7)
                .AddIngredient(ItemID.Bone, 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}