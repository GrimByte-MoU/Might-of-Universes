using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class IssueFixer : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 20;
            Item.useTime = 6;
            Item.useAnimation = 12;
            Item.reuseDelay = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 9f;
            Item.useAmmo = AmmoID.Bullet;
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Kevlar>(), 5)
                .AddIngredient(ModContent.ItemType<AcidVial>(), 8)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 5)
                .AddIngredient(ItemID.TitaniumBar, 5)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Kevlar>(), 5)
                .AddIngredient(ModContent.ItemType<AcidVial>(), 8)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 5)
                .AddIngredient(ItemID.AdamantiteBar, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
