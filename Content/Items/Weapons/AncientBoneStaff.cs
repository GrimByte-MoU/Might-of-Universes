using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Weapons
{
    public class AncientBoneStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;

            Item.DamageType = DamageClass.Summon;
            Item.damage = 40;
            Item.knockBack = 1f;
            Item.mana = 10;

            Item.buffType = ModContent.BuffType<AncientSkullMinionBuff>();
            Item.shoot = ModContent.ProjectileType<AncientSkullMinion>();

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            var spawnPos = player.Center - new Microsoft.Xna.Framework.Vector2(0, 40);
            Projectile.NewProjectile(source, spawnPos, Microsoft.Xna.Framework.Vector2.Zero, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BoneWand, 1)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 5)
                .AddIngredient(ModContent.ItemType<AncientBone>(), 10)
                .AddIngredient(ModContent.ItemType<TarChunk>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}