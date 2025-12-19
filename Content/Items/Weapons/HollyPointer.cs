using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content. Items.Projectiles;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items. Weapons
{
    public class HollyPointer : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 8);
            
            Item.useStyle = ItemUseStyleID. Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            
            Item.DamageType = DamageClass.Summon;
            Item.damage = 50;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.shoot = ModContent.ProjectileType<HollyFighter>();
            Item.buffType = ModContent.BuffType<HollyFighterBuff>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            projectile.originalDamage = Item.damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BorealWood, 50)
                .AddIngredient(ModContent.ItemType<FestiveSpirit>(), 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}