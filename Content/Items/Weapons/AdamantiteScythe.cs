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
    public class AdamantiteScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 50;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AdamantiteScytheProjectile>();
            Item.shootSpeed = 10f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(4f, target.Center); // Cal

            if (!target.active)
            {
                reaper.AddSoulEnergy(4f, target.Center); // Cal
            }
        }

public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    // Use new centralized empowerment system for projectile release
    // AdamantiteScythe fires a special projectile with no ongoing empowerment
    bool released = ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
        player, 40f, 1, // Very short duration since this is instant effect only
        new ReaperEmpowermentValues(), // No ongoing empowerment
        onConsume: (p) => {
            // Fire the special projectile when souls are consumed
            Projectile.NewProjectile(source, position, velocity, 
                ModContent.ProjectileType<AdamantiteSphereProjectile>(), 
                damage * 3, knockback, player.whoAmI);
        });
    
    return true;
}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AdamantiteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}