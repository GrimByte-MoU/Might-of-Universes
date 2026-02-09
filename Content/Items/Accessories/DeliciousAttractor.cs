using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Accessories;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class DeliciousAttractor : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.maxMinions += 1;
            player.GetDamage(DamageClass.Melee) *= 0.8f;
            player.GetDamage(DamageClass.Ranged) *= 0.8f;
            player.GetDamage(DamageClass.Magic) *= 0.8f;
            player.GetDamage(DamageClass.Summon) *= 1.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                 .AddIngredient(ModContent.ItemType<GoodnessFlower>(), 1)
              .AddIngredient(ModContent.ItemType<SweetAttractor>(), 1)
              .AddIngredient(ItemID.SoulofLight, 10)
              .AddIngredient(ItemID.IceBlock, 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}