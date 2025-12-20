using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using  MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
public class FearCaster : ModItem
{
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 70;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FearPumpkin>();
            Item.shootSpeed = 0f;
            Item.maxStack = 1;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Vector2 targetPos = Main.MouseWorld;
        Vector2 spawnPos = new Vector2(targetPos.X + Main.rand.Next(-100, 100), targetPos.Y + 600);
        Vector2 direction = targetPos - spawnPos;
        direction.Normalize();
        
        int projType = Main.rand.NextBool(20) ? ModContent.ProjectileType<PossessedPumpkin>() : type;
        Projectile.NewProjectile(source, spawnPos, direction * 12f, projType, damage, knockback, player.whoAmI);
        
        return false;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<PureTerror>(), 12)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
}
