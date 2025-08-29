using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class FrigidHeart : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FrigidHeartPlayer>().hasFrigidHeart = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LifeCrystal, 1)
                .AddIngredient(ModContent.ItemType<Materials.FrozenFragment>(), 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
