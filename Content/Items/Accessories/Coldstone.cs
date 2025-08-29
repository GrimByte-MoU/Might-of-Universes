using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class Coldstone : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Blue;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ColdstonePlayer>().hasColdstone = true;
            bool foundStone = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<ColdstoneProjectile>() && proj.owner == player.whoAmI)
                {
                    foundStone = true;
                    break;
                }
            }

            if (!foundStone)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Microsoft.Xna.Framework.Vector2.Zero, 
                    ModContent.ProjectileType<ColdstoneProjectile>(), 10, 1f, player.whoAmI);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IceBlock, 20)
                .AddIngredient(ItemID.SnowBlock, 15)
                .AddIngredient(ItemID.Shiverthorn, 3)
                .AddTile(TileID.IceMachine)
                .Register();
        }
    }
}
