using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TreatSpiker : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 44;
            Item.height = 22;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TreatSpikerCone>();
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 6f;
            Item.maxStack = 1;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => ammo.type != ItemID.WoodenArrow;

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<TreatSpikerCone>();
                velocity *= 0.6f;
                damage = (int)(damage * 1.6f);
            }
        }
    }
}
