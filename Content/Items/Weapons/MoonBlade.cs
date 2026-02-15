
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using Terraria.Audio;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class MoonBlade : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 60;
            Item.damage = 120;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MoonWave>();
            Item.shootSpeed = 20f;
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spreadAngle = MathHelper.ToRadians(5);
            
            Vector2 velocity1 = velocity.RotatedBy(-spreadAngle);
            Vector2 velocity2 = velocity.RotatedBy(spreadAngle);
            
            Projectile.NewProjectile(source, position, velocity1, type, damage, knockback, player.whoAmI, ai1: 1);
            Projectile.NewProjectile(source, position, velocity2, type, damage, knockback, player.whoAmI, ai1: -1);
            
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<LunarReap>(), 240);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}