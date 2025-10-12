using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    // Early Hardmode tome upgrade to Demon Scythe
    public class ClawoftheDamned : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.DemonScythe);
            Item.damage = 55;
            Item.mana = 12;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ModContent.ProjectileType<Projectiles.ClawDemonSickle>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemonScythe, 1)
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 5)
                .AddIngredient(ModContent.ItemType<DevilsBlood>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}