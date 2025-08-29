using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class CombatKnife : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(gold: 18);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Kevlar>(), 4)
                .AddIngredient(ModContent.ItemType<AcidVial>(), 6)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 4)
                .AddIngredient(ItemID.TitaniumBar, 6)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Kevlar>(), 4)
                .AddIngredient(ModContent.ItemType<AcidVial>(), 6)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 4)
                .AddIngredient(ItemID.AdamantiteBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
