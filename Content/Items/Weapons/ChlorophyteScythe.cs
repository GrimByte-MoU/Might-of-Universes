using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ChlorophyteScythe : ModItem
    {
        private int buffTimer = 0;
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
    var reaper = player.GetModPlayer<ReaperPlayer>();
    
    if (ReaperPlayer.SoulReleaseKey.JustPressed)
    {
        if (reaper.ConsumeSoulEnergy(140f))
        {
            player.Heal(150);
            player.lifeRegen += 15;
            buffTimer = 300; // 5 seconds
            Main.NewText("140 souls released!", Color.Green);
            return false;
        }
        else
        {
            Main.NewText("Not enough soul energy to activate!", Color.Red);
        }
    }
    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
    return false;
}

        public override void UpdateInventory(Player player)
        {
            if (buffTimer > 0)
            {
                buffTimer--;
                if (buffTimer <= 0)
                {
                player.lifeRegen -= 15;
                }
            }
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