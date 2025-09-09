using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class AncientBoneSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 80;
            Item.knockBack = 5.5f;
            Item.crit += 4;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);

            Item.shoot = ModContent.ProjectileType<AncientTeeth>();
            Item.shootSpeed = 14f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projDamage = (int)(damage * 0.5f);
            int count = Main.rand.Next(2, 5);
            float totalArcDeg = 8f;
            float halfArc = totalArcDeg * 0.5f;
            float step = count > 1 ? totalArcDeg / (count - 1) : 0f;

            Vector2 origin = player.MountedCenter;
            Vector2 baseDir = velocity.SafeNormalize(Vector2.UnitX) * Item.shootSpeed;

            for (int i = 0; i < count; i++)
            {
                float angleDeg = count > 1 ? (-halfArc + step * i) : 0f;
                Vector2 shotVel = baseDir.RotatedBy(MathHelper.ToRadians(angleDeg));

                Projectile.NewProjectile(
                    source,
                    origin,
                    shotVel,
                    ModContent.ProjectileType<AncientTeeth>(),
                    damage,
                    knockback,
                    player.whoAmI
                );
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BoneSword, 1)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 5)
                .AddIngredient(ModContent.ItemType<AncientBone>(), 10)
                .AddIngredient(ModContent.ItemType<TarChunk>(), 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}