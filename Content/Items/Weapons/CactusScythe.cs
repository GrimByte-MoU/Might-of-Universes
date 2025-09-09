using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class CactusScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 8;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(copper: 50);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CactusThorn>();
            Item.shootSpeed = 8f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(1f, target.Center); // Cal

            if (!target.active)
            {
                reaper.AddSoulEnergy(1f, target.Center); // Cal
            }
        }

public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    // Use new centralized empowerment system for spread projectile release
    bool released = ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
        player, 40f, 1, // Very short duration since this is instant effect only
        new ReaperEmpowermentValues(), // No ongoing empowerment
        onConsume: (p) => {
            // Fire 5 projectiles in a spread when souls are consumed
            for (int i = 0; i < 5; i++)
            {
                Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(-10 + (i * 5)));
                Projectile.NewProjectile(source, position, newVelocity, type, damage * 2, knockback, player.whoAmI);
            }
        });
    
    return true;
}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cactus, 15)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}

