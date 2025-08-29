using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Ammo
{
    public class EnvenomedStake : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(silver: 1, copper: 20);
            Item.rare = ItemRarityID.LightPurple;
            Item.shoot = ModContent.ProjectileType<EnvenomedStakeProjectile>();
            Item.shootSpeed = 4f;
            Item.ammo = AmmoID.Stake;
        }

        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ItemID.Stake, 10)
                .AddIngredient(ItemID.FlaskofVenom, 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}