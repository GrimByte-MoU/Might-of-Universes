using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TacticalGlaive : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.width = 28;
            Item.height = 28;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(gold: 18);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TacticalGlaiveProjectile>();
            Item.shootSpeed = 16f;
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Kevlar>(), 4)
                .AddIngredient(ModContent.ItemType<AcidVial>(), 8)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 8)
                .AddIngredient(ItemID.TitaniumBar, 8)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Kevlar>(), 4)
                .AddIngredient(ModContent.ItemType<AcidVial>(), 8)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 8)
                .AddIngredient(ItemID.AdamantiteBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
