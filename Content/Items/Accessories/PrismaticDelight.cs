using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Accessories;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class PrismaticDelight : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 2;
            player.GetDamage(DamageClass.Melee) *= 0.6f;
            player.GetDamage(DamageClass.Ranged) *= 0.6f;
            player.GetDamage(DamageClass.Magic) *= 0.6f;
            player.GetDamage(DamageClass.Summon) *= 1.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                 .AddIngredient(ModContent.ItemType<FrozenFragment>(), 10)
              .AddIngredient(ModContent.ItemType<DeliciousAttractor>(), 1)
              .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}