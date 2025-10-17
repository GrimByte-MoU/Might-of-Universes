using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Weapons
{
    public class SoundFractureBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.PulseBow);

            Item.damage = 95;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 10);
            Item.shoot = ModContent.ProjectileType<Projectiles.SoundFractureBeam>();
            Item.useAmmo = AmmoID.Arrow;
            Item.scale = 1.2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PulseBow)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}