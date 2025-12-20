using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Weapons
{
    public class LaserSaber : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BeamSword);

            Item.damage = 120;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 10);
            Item.shoot = ModContent.ProjectileType<Projectiles.LaserSaberBeam>();
            Item.ArmorPenetration = 500;
            Item.scale = 2f;
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BeamSword)
                .AddIngredient(ItemID.RedPhasesaber)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.BeamSword)
                .AddIngredient(ItemID.OrangePhasesaber)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.BeamSword)
                .AddIngredient(ItemID.YellowPhasesaber)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.BeamSword)
                .AddIngredient(ItemID.GreenPhasesaber)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.BeamSword)
                .AddIngredient(ItemID.BluePhasesaber)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.BeamSword)
                .AddIngredient(ItemID.PurplePhasesaber)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.BeamSword)
                .AddIngredient(ItemID.WhitePhasesaber)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.BeamSword)
                .AddIngredient(ItemID.BluePhasesaber)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}