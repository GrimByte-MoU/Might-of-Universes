using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Weapons
{
    public class WaferSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 80;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 3);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 25;
            Item.knockBack = 4f;
            Item.crit = 6;
            Item.shoot = ModContent.ProjectileType<WaferSwordSprinkle>();
            Item.shootSpeed = 16f;
            Item.maxStack = 1;
        }

        public override void HoldItem(Player player)
{
    Lighting.AddLight(player.Center, 1f, 1f, 1f);
}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float startingAngle = -30f;
            for (int i = 0; i < 7; i++)
            {
                Vector2 shortVelocity = velocity * 0.3f;
                Vector2 rotatedVelocity = shortVelocity.RotatedBy(MathHelper.ToRadians(startingAngle + (i * 15f)));
                Projectile.NewProjectile(source, position, rotatedVelocity, 
                    ModContent.ProjectileType<WaferSwordSprinkle>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}