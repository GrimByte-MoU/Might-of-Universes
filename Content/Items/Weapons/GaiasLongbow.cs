using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Weapons
{
    public class GaiasLongbow : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 130;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GaiasArrow>();
            Item.shootSpeed = 18f;
            Item.maxStack = 1;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projType = ModContent.ProjectileType<GaiasArrow>();

            const int shots = 4;
            const float spreadDegrees = 2f;
            for (int i = 0; i < shots; i++)
            {
                float t = shots == 1 ? 0f : (i / (float)(shots - 1));
                float angle = MathHelper.ToRadians(MathHelper.Lerp(-spreadDegrees * 0.5f, spreadDegrees * 0.5f, t));
                Vector2 perturbed = velocity.RotatedBy(angle) * Main.rand.NextFloat(0.95f, 1.05f);
                Projectile.NewProjectile(source, position, perturbed, projType, damage, knockback, player.whoAmI);
            }

            return false;
        }

    }
}
