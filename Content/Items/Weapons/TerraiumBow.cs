using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;
using System.Collections. Generic;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TerraiumBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 110;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 64;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 20f;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Arrow;
            Item.scale = 0.8f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10f, 0f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}