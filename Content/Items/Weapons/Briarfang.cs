using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Briarfang : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 165;
            Item.DamageType = DamageClass.Melee;
            Item.width = 58;
            Item.height = 58;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6.5f;
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<BriarfangProjectile>();
            
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Spear, 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 8)
                .AddIngredient(ItemID.JungleSpores, 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}