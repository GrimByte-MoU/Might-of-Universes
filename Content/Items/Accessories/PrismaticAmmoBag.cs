using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class PrismaticAmmoBag : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            
            player.GetDamage(DamageClass.Ranged) += 0.15f;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.12f;
            player.GetCritChance(DamageClass.Ranged) += 0.07f;
            player.GetModPlayer<PrismaticAmmoBagPlayer>().hasPrismaticAmmoBag = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SorcererEmblem)
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 3)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 8)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}