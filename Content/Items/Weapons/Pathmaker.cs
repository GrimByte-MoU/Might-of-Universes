using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Pathmaker : ModItem
    {
        private const int MAX_USE_TIME = 30;
        private const int MIN_USE_TIME = 15;
        private const int RAMP_INTERVAL = 60;
        private const int RAMP_AMOUNT = 3;

        private int rampTimer;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 20;
            Item.damage = 45;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = MAX_USE_TIME;
            Item.useAnimation = MAX_USE_TIME;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override void UpdateInventory(Player player)
        {
            // Only ramp while held and not using another item
            if (player.HeldItem.type == Item.type && player.itemAnimation > 0)
            {
                rampTimer++;
                if (rampTimer >= RAMP_INTERVAL)
                {
                    rampTimer = 0;
                    if (Item.useTime > MIN_USE_TIME)
                    {
                        Item.useTime = Item.useAnimation = Item.useTime - RAMP_AMOUNT;
                        if (Item.useTime < MIN_USE_TIME)
                        {
                            Item.useTime = Item.useAnimation = MIN_USE_TIME;
                        }
                    }
                }
            }
            else
            {
                // Reset when not in use
                rampTimer = 0;
                Item.useTime = Item.useAnimation = MAX_USE_TIME;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 7;
            float spread = 15f;

            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(spread));
                Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 120);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GreedySpirit>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
