using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    // Toxikarp upgrade (Post-Plantera); vanilla-like bubble behavior, faster/denser, more velocity.
    public class VenomSpewer : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Toxikarp);
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<VenomSpewerBubble>();
            Item.damage = 55;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Fire two bubbles per use with a touch of spread and speed variance
            for (int i = 0; i < 2; i++)
            {
                float rot = MathHelper.ToRadians(Main.rand.NextFloat(-4f, 4f));
                float spdScale = 1.10f + Main.rand.NextFloat(0.0f, 0.15f);
                Vector2 v = velocity.RotatedBy(rot) * spdScale;

                Projectile.NewProjectile(source, position, v, ModContent.ProjectileType<VenomSpewerBubble>(), damage, knockback, player.whoAmI);
            }
            return false; // suppress default spawn
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Toxikarp, 1)
                .AddIngredient(ModContent.ItemType<PureTerror>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}