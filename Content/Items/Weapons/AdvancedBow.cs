using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class AdvancedBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 20;
            Item.height = 40;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(gold: 22);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 18f;
            Item.crit = 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Kevlar>(), 6)
                .AddIngredient(ModContent.ItemType<AcidVial>(), 10)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 12)
                .AddIngredient(ItemID.TitaniumBar, 4)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Kevlar>(), 6)
                .AddIngredient(ModContent.ItemType<AcidVial>(), 10)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 12)
                .AddIngredient(ItemID.AdamantiteBar, 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
