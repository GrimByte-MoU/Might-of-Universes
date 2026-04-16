using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class RazorLeaves : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 105;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 5;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.shoot = ModContent.ProjectileType<RazorLeaf>();
            Item.shootSpeed = 16f;
            Item.mana = 4;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.reuseDelay = 0;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
            
            float speedMultiplier = Main.rand.NextFloat(0.8f, 1.2f);
            velocity *= speedMultiplier;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int leafCount = Main.rand.Next(1, 3);
            
            for (int i = 0; i < leafCount; i++)
            {
                Vector2 perturbedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(0.9f, 1.1f);
                Projectile.NewProjectile(source, position, perturbedVelocity, type, damage, knockback, player.whoAmI);
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpellTome)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddIngredient(ItemID.ChlorophyteBar, 5)
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}