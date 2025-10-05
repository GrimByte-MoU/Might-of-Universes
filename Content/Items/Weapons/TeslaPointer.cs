using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TeslaPointer : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 85;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TeslaPointerBolt>();
            Item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Small forward offset so it never spawns “inside” player & immediately collides.
            position += velocity.SafeNormalize(Vector2.UnitX) * 12f;

            float amplitude = 70f; // peak lateral offset in pixels (tweak)
            float travelTime = 36f; // ticks; matches projectile timeLeft

            // Left arc
            Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI,
                ai0: amplitude,
                ai1: 1f,   // direction +1
                ai2: travelTime
            );
            // Right arc
            Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI,
                ai0: amplitude,
                ai1: -1f,  // direction -1
                ai2: travelTime
            );
            return false; // we handled spawning both
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wire, 100)
                .AddIngredient(ItemID.Cog, 30)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 20)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 300);
        }
    }
}