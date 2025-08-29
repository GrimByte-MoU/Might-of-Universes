using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ArcaneGladius : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 72;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Pink;
            Item.mana = 10;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.noUseGraphic = false;
            Item.channel = false;
            Item.UseSound = SoundID.Item43;
            Item.shoot = ModContent.ProjectileType<ArcaneGladiusProjectile>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 weaponDir = Vector2.Normalize(Main.MouseWorld - player.Center);
            float thrustDistance = 40f;
            Vector2 spawnPos = player.Center + weaponDir * thrustDistance;
            velocity = weaponDir * Item.shootSpeed;

            Projectile.NewProjectile(source, spawnPos, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
