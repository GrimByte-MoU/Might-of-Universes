using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class UnderworldSorcererRobes : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.06f;
            player.GetCritChance(DamageClass.Magic) += 5f;
            player.manaCost -= 0.10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 12)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddIngredient(ItemID.Bone, 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}