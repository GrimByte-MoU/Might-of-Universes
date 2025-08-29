using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class Lavastone : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 7);
            Item.rare = ItemRarityID.Green;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<LavastonePlayer>().hasLavastone = true;
            bool foundStone = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<LavastoneProjectile>() && proj.owner == player.whoAmI)
                {
                    foundStone = true;
                    break;
                }
            }

            if (!foundStone)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Microsoft.Xna.Framework.Vector2.Zero, 
                    ModContent.ProjectileType<LavastoneProjectile>(), 15, 1f, player.whoAmI);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 15)
                .AddIngredient(ItemID.AshBlock, 15)
                .AddIngredient(ItemID.Fireblossom, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}