using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Common.Systems;

namespace MightofUniverses.Content.Items.Weapons
{
    public class DataPiercer : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 24;
            Item.height = 48;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position,
                                   Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                for (int i = 0; i < 2; i++)
                {
                    DataZapSystem.TryZapEnemies(player, position, velocity, damage, knockback);
                }
                return false;
            }

            float spread = 0.1f;
            for (int i = 0; i < 2; i++)
            {
                Vector2 perturbedVelocity = velocity.RotatedByRandom(spread);
                Projectile.NewProjectile(source, position, perturbedVelocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
