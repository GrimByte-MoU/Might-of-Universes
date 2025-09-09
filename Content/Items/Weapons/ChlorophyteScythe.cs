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
    public class ChlorophyteScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 55;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ChlorophyteScytheProjectile>();
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
    // Use new centralized empowerment system - instant heal + life regen
    var empowermentValues = new ReaperEmpowermentValues
    {
        LifeRegen = 15
    };
    bool released = ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(player, 140f, 300, empowermentValues,
        onConsume: (p) => {
            // Apply instant healing when souls are consumed
            p.Heal(150);
        });
    
    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
    return false;
}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}