using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses. Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses. Content.Items.Weapons
{
    public class AirshipBeacon : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            ItemID.Sets.StaffMinionSlotsRequired[Type] = 5f;
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.knockBack = 3f;
            Item.mana = 20;
            Item.width = 44;
            Item.height = 44;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID. Swing;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID. Lime;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<ClockworkAirshipBuff>();
            Item.shoot = ModContent.ProjectileType<ClockworkAirship>();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == Item.shoot && Main.projectile[i].owner == player.whoAmI)
                {
                    Main.projectile[i].Kill();
                }
            }

            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            projectile.originalDamage = Item.damage;

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrassBar>(), 25)
                .AddIngredient(ItemID.SoulofFlight, 20)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddIngredient(ItemID. Cog, 100)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}