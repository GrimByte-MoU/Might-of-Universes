using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TeslaPointer : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 85;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TeslaPointerBolt>();
            Item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    // Shoot two projectiles that curve outward and back in
    float spreadAngle = MathHelper.ToRadians(15);
    
    Vector2 velocity1 = velocity.RotatedBy(-spreadAngle);
    Vector2 velocity2 = velocity.RotatedBy(spreadAngle);
    
    Projectile.NewProjectile(source, position, velocity1, type, damage, knockback, player.whoAmI, ai1: 1);
    Projectile.NewProjectile(source, position, velocity2, type, damage, knockback, player.whoAmI, ai1: -1);
    
    return false;
}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 300);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wire, 100)
                .AddIngredient(ItemID.Cog, 30)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 20)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

