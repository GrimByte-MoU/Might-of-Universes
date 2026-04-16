using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Weapons
{
    public class WaferBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 44;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item5;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 28;
            Item.knockBack = 3f;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 14f;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 80);
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 arrow1 = velocity.RotatedBy(MathHelper.ToRadians(-3f));
            Vector2 arrow2 = velocity.RotatedBy(MathHelper.ToRadians(3f));

            Projectile.NewProjectile(source, position, arrow1, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, arrow2, type, damage, knockback, player.whoAmI);

            return false;
        }
    }

    public class WaferBowGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[projectile.owner];

            if (owner.HeldItem.type != ModContent.ItemType<WaferBow>())
                return;

            if (!projectile.arrow)
                return;

            target.AddBuff(ModContent.BuffType<SugarCrash>(), 120);
        }
    }
}