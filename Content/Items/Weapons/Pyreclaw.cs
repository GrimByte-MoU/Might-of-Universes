using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Pyreclaw : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 160;
            Item.DamageType = DamageClass.Melee;
            Item.width = 80;
            Item.height = 80;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<PyreclawFireball>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 direction = velocity.SafeNormalize(Vector2.UnitX * player.direction);
            
            Projectile.NewProjectile(
                source,
                player.Center,
                direction * Item.shootSpeed,
                type,
                (int)(damage * 1),
                knockback,
                player.whoAmI
            );

            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Torch, 0f, 0f, 100, Color.OrangeRed, 1.5f);
                dust.noGravity = true;
            }

            Lighting.AddLight(player.Center, 1.2f, 0.6f, 0f);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Buffs.CoreHeat>(), 120);

            for (int d = 0; d < 10; d++)
            {
                Dust dust = Dust.NewDustDirect(target.Center, 10, 10, DustID.Torch, 0, 0, 100, Color.OrangeRed, 2f);
                dust.noGravity = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FieryGreatsword, 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}