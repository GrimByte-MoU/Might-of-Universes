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
    public class OrichalcumScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 45;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<OrichalcumScytheProjectile>();
            Item.shootSpeed = 10f;
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
    // Use new centralized empowerment system - attack speed + reaper damage
    var empowermentValues = new ReaperEmpowermentValues
    {
        AttackSpeed = 0.15f,
        ReaperDamage = 0.15f
    };
    ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(player, 40f, 240, empowermentValues);

    position.Y -= Item.height / 2;
    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
    return false;
}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.OrichalcumBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}