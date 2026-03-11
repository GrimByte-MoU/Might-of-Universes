using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Weapons
{
    public class SoundFractureBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.PulseBow);

            Item.damage = 100;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 10);
            Item.shoot = ModContent.ProjectileType<SoundFractureBeam>();
            Item.useAmmo = AmmoID.Arrow;
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<SoundFractureBeam>();
            }

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            
            return false;
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