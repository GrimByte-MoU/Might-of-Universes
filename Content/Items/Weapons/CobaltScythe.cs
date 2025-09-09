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
    public class CobaltScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 40;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CobaltScytheProjectile>();
            Item.shootSpeed = 8f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(3f, target.Center); // Cal

            if (!target.active)
            {
                reaper.AddSoulEnergy(3f, target.Center); // Cal
            }
        }

public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    // Use new centralized empowerment system for projectile release
    bool released = ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
        player, 40f, 1, // Very short duration since this is instant effect only
        new ReaperEmpowermentValues(), // No ongoing empowerment
        onConsume: (p) => {
            // Fire multiple cobalt shots when souls are consumed
            for (int i = -1; i <= 2; i++)
            {
                Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(10 * i));
                Projectile.NewProjectile(source, position, newVelocity, 
                    ModContent.ProjectileType<CobaltShotProjectile>(), 
                    damage * 2, knockback, player.whoAmI);
            }
        });
    
    return true;
}




        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CobaltBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}