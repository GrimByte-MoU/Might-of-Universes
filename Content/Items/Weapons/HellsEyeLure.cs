using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class HellsEyeLure : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(gold: 4);
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;

            Item.mana = 10;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 24;
            Item.knockBack = 2f;

            Item.buffType = ModContent.BuffType<HellsEyeBuff>();
            Item.shoot = ModContent.ProjectileType<HellsEyeMinion>();
            Item.shootSpeed = 0f; // minions ignore shootSpeed
            Item.maxStack = 1;
        }

        public override bool CanUseItem(Player player)
        {
            int existing = player.ownedProjectileCounts[ModContent.ProjectileType<HellsEyeMinion>()];
            return existing < 12;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            int existing = player.ownedProjectileCounts[ModContent.ProjectileType<HellsEyeMinion>()];
            int canSpawn = 12 - existing;
            int toSpawn = Utils.Clamp(4, 0, canSpawn);

            for (int i = 0; i < toSpawn; i++)
            {
                Vector2 spawnPos = player.Center + Main.rand.NextVector2Circular(32f, 32f);
                int proj = Projectile.NewProjectile(source, spawnPos, Vector2.Zero, type, damage, knockback, player.whoAmI);
                if (proj >= 0 && proj < Main.maxProjectiles)
                {
                    Main.projectile[proj].originalDamage = damage;
                }
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 10)
                .AddIngredient(ModContent.ItemType<DevilsBlood>(), 15)
                .AddIngredient(ItemID.BeeWax, 7)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}